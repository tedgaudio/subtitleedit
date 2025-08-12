using Nikse.SubtitleEdit.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nikse.SubtitleEdit.Core.SubtitleFormats
{
    public class GaudioJson : SubtitleFormat
    {
        public override string Extension => ".json";

        public override string Name => "Gaudio JSON";

        public override bool IsMine(List<string> lines, string fileName)
        {
            var sb = new StringBuilder();
            foreach (var s in lines)
            {
                sb.Append(s);
            }

            var jsonText = sb.ToString().Trim();
            
            // Gaudio JSON 포맷 감지: metadata와 subtitles를 포함하는지 확인
            return jsonText.StartsWith("{\"metadata\"", StringComparison.Ordinal) && 
                   jsonText.Contains("\"subtitles\":", StringComparison.Ordinal);
        }

        public static string EncodeJsonText(string text, string newLineCharacter = "<br />")
        {
            var sb = new StringBuilder(text.Length);
            foreach (var c in text)
            {
                switch (c)
                {
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '"':
                        sb.Append("\\\"");
                        break;
                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString().Replace(Environment.NewLine, newLineCharacter);
        }

        public static string DecodeJsonText(string text)
        {
            text = text.Replace("<br />", Environment.NewLine);
            text = text.Replace("<br>", Environment.NewLine);
            text = text.Replace("<br/>", Environment.NewLine);
            text = text.Replace("\\r\\n", Environment.NewLine);
            text = text.Replace("\\n", Environment.NewLine);
            var sb = new StringBuilder(text.Length);
            var list = text.SplitToLines();
            for (var index = 0; index < list.Count; index++)
            {
                var line = list[index];
                DecodeJsonText(line, sb);
                if (index <  list.Count - 1)
                {
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        private static void DecodeJsonText(string text, StringBuilder sb)
        {
            text = string.Join(Environment.NewLine, text.SplitToLines());
            var keepNext = false;
            var hexLetters = "01234567890abcdef";
            var i = 0;
            while (i < text.Length)
            {
                char c = text[i];
                if (c == '\\' && !keepNext)
                {
                    keepNext = true;
                    if (i + 5 < text.Length && text[i + 1] == 'u' &&
                        hexLetters.Contains(text[i + 2]) &&
                        hexLetters.Contains(text[i + 3]) &&
                        hexLetters.Contains(text[i + 4]) &&
                        hexLetters.Contains(text[i + 5]))
                    {
                        var unicodeString = text.Substring(i, 6);
                        var unescaped = System.Text.RegularExpressions.Regex.Unescape(unicodeString);
                        sb.Append(unescaped);
                        i += 5;
                    }
                }
                else
                {
                    sb.Append(c);
                    keepNext = false;
                }

                i++;
            }
        }

        public override string ToText(Subtitle subtitle, string title)
        {
            var sb = new StringBuilder();
            
            sb.AppendLine("{");
            sb.AppendLine("  \"metadata\": {");
            sb.AppendLine("    \"generator\": \"Gaudio Subtitle Edit\",");
            sb.AppendLine("    \"version\": \"" + "4.0.12.4d9edc1" + "\",");
            //sb.AppendLine("    \"version\": \"" + Utilities.AssemblyVersion + "\",");
            sb.AppendLine("    \"updated\": \"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\"");
            sb.AppendLine("  },");
            sb.AppendLine("  \"subtitles\": [");
            
            var count = 0;
            foreach (var p in subtitle.Paragraphs)
            {
                if (count > 0)
                {
                    sb.Append(',');
                }

                sb.AppendLine();
                sb.Append("    {\"start\":");
                sb.Append(p.StartTime.TotalSeconds.ToString("F3", System.Globalization.CultureInfo.InvariantCulture));
                sb.Append(",\"end\":");
                sb.Append(p.EndTime.TotalSeconds.ToString("F3", System.Globalization.CultureInfo.InvariantCulture));
                sb.Append(",\"text\":\"");
                sb.Append(EncodeJsonText(p.Text));
                sb.Append("\"");
                
                // Add custom fields
                if (!string.IsNullOrEmpty(p.Actor))
                {
                    sb.Append(",\"actor\":\"");
                    sb.Append(EncodeJsonText(p.Actor));
                    sb.Append("\"");
                }
                
                if (!string.IsNullOrEmpty(p.OnOff_Screen))
                {
                    sb.Append(",\"onOffScreen\":\"");
                    sb.Append(EncodeJsonText(p.OnOff_Screen));
                    sb.Append("\"");
                }
                
                if (!string.IsNullOrEmpty(p.Diegetic))
                {
                    sb.Append(",\"diegetic\":\"");
                    sb.Append(EncodeJsonText(p.Diegetic));
                    sb.Append("\"");
                }
                
                if (!string.IsNullOrEmpty(p.Notes))
                {
                    sb.Append(",\"notes\":\"");
                    sb.Append(EncodeJsonText(p.Notes));
                    sb.Append("\"");
                }
                
                if (!string.IsNullOrEmpty(p.DialogueReverb))
                {
                    sb.Append(",\"dialogueReverb\":\"");
                    sb.Append(EncodeJsonText(p.DialogueReverb));
                    sb.Append("\"");
                }
                
                if (!string.IsNullOrEmpty(p.DFX))
                {
                    sb.Append(",\"dfx\":\"");
                    sb.Append(EncodeJsonText(p.DFX));
                    sb.Append("\"");
                }
                
                sb.Append("}");
                count++;
            }
            sb.AppendLine();
            sb.AppendLine("  ]");
            sb.AppendLine("}");
            return sb.ToString().Trim();
        }

        public override void LoadSubtitle(Subtitle subtitle, List<string> lines, string fileName)
        {
            _errorCount = 0;

            var sb = new StringBuilder();
            foreach (var s in lines)
            {
                sb.Append(s);
            }

            var jsonText = sb.ToString().Trim();
            
            // Check if it's the new format with metadata
            if (jsonText.StartsWith("{\"metadata\"", StringComparison.Ordinal))
            {
                LoadSubtitleNewFormat(subtitle, jsonText);
            }
            else
            {
                return;
            }
        }

        private void LoadSubtitleNewFormat(Subtitle subtitle, string jsonText)
        {
            try
            {
                // Extract subtitles array from the new format
                var subtitlesStart = jsonText.IndexOf("\"subtitles\":[", StringComparison.Ordinal);
                if (subtitlesStart < 0)
                {
                    return;
                }
                
                var subtitlesContent = jsonText.Substring(subtitlesStart + 12); // Skip "subtitles":[
                var subtitlesEnd = FindMatchingBracket(subtitlesContent, 0);
                if (subtitlesEnd < 0)
                {
                    return;
                }
                
                var subtitlesArray = "[" + subtitlesContent.Substring(0, subtitlesEnd) + "]";
                LoadSubtitleOldFormat(subtitle, subtitlesArray);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error parsing new format: {ex.Message}");
                // Fallback to old format parsing
                LoadSubtitleOldFormat(subtitle, jsonText);
            }
        }

        private int FindMatchingBracket(string text, int startIndex)
        {
            var braceCount = 0;
            var inString = false;
            var escapeNext = false;
            
            for (int i = startIndex; i < text.Length; i++)
            {
                char c = text[i];
                
                if (escapeNext)
                {
                    escapeNext = false;
                    continue;
                }
                
                if (c == '\\')
                {
                    escapeNext = true;
                    continue;
                }
                
                if (c == '"' && !escapeNext)
                {
                    inString = !inString;
                }
                
                if (!inString)
                {
                    if (c == '[')
                    {
                        braceCount++;
                    }
                    else if (c == ']')
                    {
                        braceCount--;
                        if (braceCount == 0)
                        {
                            return i;
                        }
                    }
                }
            }
            
            return -1;
        }

        private void LoadSubtitleOldFormat(Subtitle subtitle, string jsonText)
        {

            // JSON 파싱을 더 안정적으로 처리
            try
            {
                // 각 JSON 객체를 개별적으로 파싱
                var jsonObjects = ParseJsonObjects(jsonText);
                
                foreach (var jsonObj in jsonObjects)
                {
                    string start = ReadTag(jsonObj, "start");
                    string end = ReadTag(jsonObj, "end");
                    string text = ReadTag(jsonObj, "text");
                    
                    if (start != null && end != null && text != null && !IsTagArray(jsonObj, "text"))
                    {
                        if (double.TryParse(start, System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture, out var startSeconds) &&
                            double.TryParse(end, System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture, out var endSeconds))
                        {
                            var logMessage = $"GaudioJson: start={start}, startSeconds={startSeconds}, startMilliseconds={startSeconds * TimeCode.BaseUnit}";
                            System.Diagnostics.Debug.WriteLine(logMessage);
                            System.Console.WriteLine(logMessage);
                            System.IO.File.AppendAllText("gaudio_debug.log", logMessage + Environment.NewLine);
                            
                            logMessage = $"GaudioJson: end={end}, endSeconds={endSeconds}, endMilliseconds={endSeconds * TimeCode.BaseUnit}";
                            System.Diagnostics.Debug.WriteLine(logMessage);
                            System.Console.WriteLine(logMessage);
                            System.IO.File.AppendAllText("gaudio_debug.log", logMessage + Environment.NewLine);
                            
                            var p = new Paragraph(DecodeJsonText(text), startSeconds * TimeCode.BaseUnit, endSeconds * TimeCode.BaseUnit);
                            
                            logMessage = $"GaudioJson: Created paragraph with StartTime={p.StartTime.TotalMilliseconds}, EndTime={p.EndTime.TotalMilliseconds}";
                            System.Diagnostics.Debug.WriteLine(logMessage);
                            System.Console.WriteLine(logMessage);
                            System.IO.File.AppendAllText("gaudio_debug.log", logMessage + Environment.NewLine);
                            
                            // Read custom fields with better error handling and debugging
                            try
                            {
                                string actor = ReadTag(jsonObj, "actor");
                                if (!string.IsNullOrEmpty(actor))
                                {
                                    p.Actor = DecodeJsonText(actor);
                                    System.Diagnostics.Debug.WriteLine($"Loaded actor: {p.Actor}");
                                }
                                
                                string onOffScreen = ReadTag(jsonObj, "onOffScreen");
                                if (!string.IsNullOrEmpty(onOffScreen))
                                {
                                    p.OnOff_Screen = DecodeJsonText(onOffScreen);
                                    System.Diagnostics.Debug.WriteLine($"Loaded onOffScreen: {p.OnOff_Screen}");
                                }
                                
                                string diegetic = ReadTag(jsonObj, "diegetic");
                                if (!string.IsNullOrEmpty(diegetic))
                                {
                                    p.Diegetic = DecodeJsonText(diegetic);
                                    System.Diagnostics.Debug.WriteLine($"Loaded diegetic: {p.Diegetic}");
                                }
                                
                                string notes = ReadTag(jsonObj, "notes");
                                if (!string.IsNullOrEmpty(notes))
                                {
                                    p.Notes = DecodeJsonText(notes);
                                    System.Diagnostics.Debug.WriteLine($"Loaded notes: {p.Notes}");
                                }
                                
                                string dialogueReverb = ReadTag(jsonObj, "dialogueReverb");
                                if (!string.IsNullOrEmpty(dialogueReverb))
                                {
                                    p.DialogueReverb = DecodeJsonText(dialogueReverb);
                                    System.Diagnostics.Debug.WriteLine($"Loaded dialogueReverb: {p.DialogueReverb}");
                                }
                                
                                string dfx = ReadTag(jsonObj, "dfx");
                                if (!string.IsNullOrEmpty(dfx))
                                {
                                    p.DFX = DecodeJsonText(dfx);
                                    System.Diagnostics.Debug.WriteLine($"Loaded dfx: {p.DFX}");
                                }
                            }
                            catch (Exception ex)
                            {
                                // 로그 또는 디버깅 정보를 여기에 추가할 수 있습니다
                                System.Diagnostics.Debug.WriteLine($"Error parsing custom fields: {ex.Message}");
                                System.Diagnostics.Debug.WriteLine($"JSON object: {jsonObj}");
                            }
                            
                            subtitle.Paragraphs.Add(p);
                        }
                        else
                        {
                            _errorCount++;
                        }
                    }
                    else
                    {
                        _errorCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                // JSON 파싱 중 오류 발생 시 기존 방식으로 fallback
                System.Diagnostics.Debug.WriteLine($"JSON parsing error, falling back to old method: {ex.Message}");
                LoadSubtitleFallback(subtitle, jsonText);
            }
            
            subtitle.Renumber();
        }

        private List<string> ParseJsonObjects(string jsonText)
        {
            var objects = new List<string>();
            var currentObject = new StringBuilder();
            var braceCount = 0;
            var inString = false;
            var escapeNext = false;
            
            for (int i = 0; i < jsonText.Length; i++)
            {
                char c = jsonText[i];
                
                if (escapeNext)
                {
                    currentObject.Append(c);
                    escapeNext = false;
                    continue;
                }
                
                if (c == '\\')
                {
                    escapeNext = true;
                    currentObject.Append(c);
                    continue;
                }
                
                if (c == '"' && !escapeNext)
                {
                    inString = !inString;
                }
                
                if (!inString)
                {
                    if (c == '{')
                    {
                        if (braceCount == 0)
                        {
                            currentObject.Clear();
                        }
                        braceCount++;
                    }
                    else if (c == '}')
                    {
                        braceCount--;
                        if (braceCount == 0)
                        {
                            currentObject.Append(c);
                            var obj = currentObject.ToString().Trim();
                            if (obj.Length > 2) // 최소한 "{}" 보다 커야 함
                            {
                                objects.Add(obj);
                            }
                            currentObject.Clear();
                            continue;
                        }
                    }
                }
                
                if (braceCount > 0)
                {
                    currentObject.Append(c);
                }
            }
            
            return objects;
        }

        private void LoadSubtitleFallback(Subtitle subtitle, string jsonText)
        {
            // 기존의 로드 방식을 fallback으로 사용
            foreach (string line in jsonText.Replace("},{", Environment.NewLine).SplitToLines())
            {
                string s = line.Trim() + "}";
                string start = ReadTag(s, "start");
                string end = ReadTag(s, "end");
                string text = ReadTag(s, "text");
                if (start != null && end != null && text != null && !IsTagArray(s, "text"))
                {
                    if (double.TryParse(start, System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture, out var startSeconds) &&
                        double.TryParse(end, System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture, out var endSeconds))
                    {
 
                        var logMessage = $"GaudioJson Fallback: start={start}, startSeconds={startSeconds}, startMilliseconds={startSeconds * TimeCode.BaseUnit}";
                        System.Diagnostics.Debug.WriteLine(logMessage);
                        System.IO.File.AppendAllText("gaudio_debug.log", logMessage + Environment.NewLine);
                        
                        logMessage = $"GaudioJson Fallback: end={end}, endSeconds={endSeconds}, endMilliseconds={endSeconds * TimeCode.BaseUnit}";
                        System.Diagnostics.Debug.WriteLine(logMessage);
                        System.IO.File.AppendAllText("gaudio_debug.log", logMessage + Environment.NewLine);
                        
                        var p = new Paragraph(DecodeJsonText(text), startSeconds * TimeCode.BaseUnit, endSeconds * TimeCode.BaseUnit);
                        
                        logMessage = $"GaudioJson Fallback: Created paragraph with StartTime={p.StartTime.TotalMilliseconds}, EndTime={p.EndTime.TotalMilliseconds}";
                        System.Diagnostics.Debug.WriteLine(logMessage);
                        System.IO.File.AppendAllText("gaudio_debug.log", logMessage + Environment.NewLine);
                        
                        // Read custom fields
                        string actor = ReadTag(s, "actor");
                        if (!string.IsNullOrEmpty(actor))
                        {
                            p.Actor = DecodeJsonText(actor);
                        }
                        
                        string onOffScreen = ReadTag(s, "onOffScreen");
                        if (!string.IsNullOrEmpty(onOffScreen))
                        {
                            p.OnOff_Screen = DecodeJsonText(onOffScreen);
                        }
                        
                        string diegetic = ReadTag(s, "diegetic");
                        if (!string.IsNullOrEmpty(diegetic))
                        {
                            p.Diegetic = DecodeJsonText(diegetic);
                        }
                        
                        string notes = ReadTag(s, "notes");
                        if (!string.IsNullOrEmpty(notes))
                        {
                            p.Notes = DecodeJsonText(notes);
                        }
                        
                        string dialogueReverb = ReadTag(s, "dialogueReverb");
                        if (!string.IsNullOrEmpty(dialogueReverb))
                        {
                            p.DialogueReverb = DecodeJsonText(dialogueReverb);
                        }
                        
                        string dfx = ReadTag(s, "dfx");
                        if (!string.IsNullOrEmpty(dfx))
                        {
                            p.DFX = DecodeJsonText(dfx);
                        }
                        
                        subtitle.Paragraphs.Add(p);
                    }
                    else
                    {
                        _errorCount++;
                    }
                }
                else
                {
                    _errorCount++;
                }
            }
        }

        private static bool IsTagArray(string content, string tag)
        {
            var startIndex = content.IndexOfAny(new[] { "\"" + tag + "\"", "'" + tag + "'" }, StringComparison.Ordinal);
            if (startIndex < 0)
            {
                return false;
            }

            return content.Substring(startIndex + 3 + tag.Length).Trim().TrimStart(':').TrimStart().StartsWith('[');
        }

        public static string ConvertJsonSpecialCharacters(string s)
        {
            if (s.Contains("\\u00"))
            {
                for (int i = 33; i < 200; i++)
                {
                    var tag = "\\u" + i.ToString("x4");
                    if (s.Contains(tag))
                    {
                        s = s.Replace(tag, Convert.ToChar(i).ToString());
                    }
                }
            }
            return s;
        }

        private static readonly char[] CommaAndEndCurlyBracket = { ',', '}' };

        public static string ReadTag(string s, string tag)
        {
            // 여러 가능한 필드명을 시도
            var possibleTags = new[] { tag, tag.ToLower(), tag.ToUpper(), tag.Replace("_", ""), tag.Replace("_", " ") };
            
            foreach (var possibleTag in possibleTags)
            {
                var startIndex = s.IndexOfAny(new[] { "\"" + possibleTag + "\"", "'" + possibleTag + "'" }, StringComparison.Ordinal);
                if (startIndex < 0)
                {
                    continue;
                }

                if (startIndex + 3 + possibleTag.Length > s.Length)
                {
                    continue;
                }

                var res = s.Substring(startIndex + 3 + possibleTag.Length).Trim().TrimStart(':').TrimStart();
                if (res.StartsWith('"'))
                { // text
                    res = ConvertJsonSpecialCharacters(res);
                    res = res.Replace("\\\"", "@__1");
                    int endIndex = res.IndexOf("\"}", StringComparison.Ordinal);
                    if (endIndex == -1)
                    {
                        endIndex = res.LastIndexOf('"');
                    }
                    int endAlternate = res.IndexOf("\",", StringComparison.Ordinal);
                    if (endIndex < 0)
                    {
                        endIndex = endAlternate;
                    }
                    else if (endAlternate > 0 && endAlternate < endIndex)
                    {
                        endIndex = endAlternate;
                    }

                    if (endIndex < 0 && res.EndsWith("\"", StringComparison.Ordinal))
                    {
                        endIndex = res.Length - 1;
                    }

                    if (endIndex <= 0)
                    {
                        continue;
                    }

                    if (res.Length > 1)
                    {
                        var result = res.Substring(1, endIndex - 1).Replace("@__1", "\\\"");
                        if (!string.IsNullOrEmpty(result))
                        {
                            System.Diagnostics.Debug.WriteLine($"Found tag '{possibleTag}' with value: {result}");
                            return result;
                        }
                    }

                    return string.Empty;
                }
                else
                { // number
                    var endIndex = res.IndexOfAny(CommaAndEndCurlyBracket);
                    if (endIndex < 0)
                    {
                        continue;
                    }

                    var result = res.Substring(0, endIndex);
                    if (!string.IsNullOrEmpty(result))
                    {
                        System.Diagnostics.Debug.WriteLine($"Found tag '{possibleTag}' with value: {result}");
                        return result;
                    }
                }
            }
            
            return null;
        }

        public static List<string> ReadArray(string s, string tag)
        {
            var list = new List<string>();

            var startIndex = s.IndexOfAny(new[] { "\"" + tag + "\"", "'" + tag + "'" }, StringComparison.Ordinal);
            if (startIndex < 0)
            {
                return list;
            }

            startIndex += tag.Length + 2;
            string res = s.Substring(startIndex).TrimStart().TrimStart(':').TrimStart();
            int tagLevel = 1;
            int oldStart = 0;
            if (oldStart < res.Length && res[oldStart] == '[')
            {
                oldStart++;
            }
            int nextTag = oldStart;
            while (tagLevel >= 1 && nextTag >= 0 && nextTag + 1 < res.Length)
            {
                while (oldStart < res.Length && res[oldStart] == ' ')
                {
                    oldStart++;
                }

                if (oldStart < res.Length && res[oldStart] == '"')
                {
                    nextTag = res.IndexOf('"', oldStart + 1);

                    while (nextTag > 0 && nextTag + 1 < res.Length && res[nextTag - 1] == '\\')
                    {
                        nextTag = res.IndexOf('"', nextTag + 1);
                    }

                    if (nextTag > 0)
                    {
                        string newValue = res.Substring(oldStart, nextTag - oldStart);
                        list.Add(newValue.Remove(0, 1));
                        oldStart = nextTag + 1;
                        while (oldStart < res.Length && "\r\n\t ".Contains(res[oldStart]))
                        {
                            oldStart++;
                        }
                        if (oldStart < res.Length && res[oldStart] == ']')
                        {
                            oldStart++;
                        }
                        while (oldStart < res.Length && "\r\n\t ".Contains(res[oldStart]))
                        {
                            oldStart++;
                        }
                        if (oldStart < res.Length && res[oldStart] == ',')
                        {
                            oldStart++;
                        }
                        while (oldStart < res.Length && "\r\n\t ".Contains(res[oldStart]))
                        {
                            oldStart++;
                        }
                        if (oldStart < res.Length && res[oldStart] == '[')
                        {
                            oldStart++;
                        }
                        while (oldStart < res.Length && "\r\n\t ".Contains(res[oldStart]))
                        {
                            oldStart++;
                        }
                    }
                }
                else if (oldStart < res.Length && res[oldStart] != '[' && res[oldStart] != ']')
                {
                    nextTag = res.IndexOf(',', oldStart + 1);
                    if (nextTag > 0)
                    {
                        string newValue = res.Substring(oldStart, nextTag - oldStart);
                        if (newValue.EndsWith(']'))
                        {
                            newValue = newValue.TrimEnd(']');
                            tagLevel = -10; // return
                        }
                        list.Add(newValue.Trim());
                        oldStart = nextTag + 1;
                    }
                }
                else
                {
                    int nextBegin = res.IndexOf('[', nextTag);
                    int nextEnd = res.IndexOf(']', nextTag);
                    if (nextBegin < nextEnd && nextBegin != -1)
                    {
                        nextTag = nextBegin + 1;
                        tagLevel++;
                    }
                    else
                    {
                        nextTag = nextEnd + 1;
                        tagLevel--;
                        if (tagLevel == 1)
                        {
                            string newValue = res.Substring(oldStart, nextTag - oldStart);
                            list.Add(newValue);
                            if (res[nextTag] == ']')
                            {
                                tagLevel--;
                            }

                            oldStart = nextTag + 1;
                        }
                    }
                }
            }
            return list;
        }

        internal static List<string> ReadArray(string text)
        {
            var list = new List<string>();
            text = text.Trim();
            if (text.StartsWith('[') && text.EndsWith(']'))
            {
                text = text.Trim('[', ']');
                text = text.Trim();

                text = text.Replace("<br />", Environment.NewLine);
                text = text.Replace("<br>", Environment.NewLine);
                text = text.Replace("<br/>", Environment.NewLine);
                text = text.Replace("\\n", Environment.NewLine);

                bool keepNext = false;
                var sb = new StringBuilder();
                foreach (var c in text)
                {
                    if (c == '\\' && !keepNext)
                    {
                        keepNext = true;
                    }
                    else if (!keepNext && c == ',')
                    {
                        list.Add(sb.ToString());
                        sb.Clear();
                    }
                    else
                    {
                        sb.Append(c);
                        keepNext = false;
                    }
                }
                if (sb.Length > 0)
                {
                    list.Add(sb.ToString());
                }
            }
            return list;
        }

        public static List<string> ReadObjectArray(string text)
        {
            var list = new List<string>();
            text = text.Trim();
            if (text.StartsWith('[') && text.EndsWith(']'))
            {
                text = text.Trim('[', ']').Trim();
                int onCount = 0;
                bool keepNext = false;
                var sb = new StringBuilder();
                foreach (var c in text)
                {
                    if (keepNext)
                    {
                        sb.Append(c);
                        keepNext = false;
                    }
                    else if (c == '\\')
                    {
                        sb.Append(c);
                        keepNext = true;
                    }
                    else if (c == '{')
                    {
                        sb.Append(c);
                        onCount++;
                    }
                    else if (c == '}')
                    {
                        sb.Append(c);
                        onCount--;
                    }
                    else if (c == ',' && onCount == 0)
                    {
                        list.Add(sb.ToString().Trim());
                        sb.Clear();
                    }
                    else
                    {
                        sb.Append(c);
                    }
                }
                if (sb.Length > 0)
                {
                    list.Add(sb.ToString().Trim());
                }
            }
            return list;
        }
    }
}
