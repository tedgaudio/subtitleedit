using Nikse.SubtitleEdit.Core.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nikse.SubtitleEdit.Core.SubtitleFormats
{
    public class GaudioJsonMetadata
    {
        [JsonProperty("gaudioJsonVersion")]
        public string GaudioJsonVersion { get; set; }
        
        [JsonProperty("subtitleEditVersion")]
        public string SubtitleEditVersion { get; set; }
        
        [JsonProperty("format")]
        public string Format { get; set; }
        
        [JsonProperty("created")]
        public string Created { get; set; }
        
        [JsonProperty("modified")]
        public string Modified { get; set; }
        
        [JsonProperty("subtitleCount")]
        public int SubtitleCount { get; set; }
        
        [JsonProperty("totalDuration")]
        public double TotalDuration { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
    }

    public class GaudioJsonTranscription
    {
        [JsonProperty("index")]
        public int Index { get; set; }
        
        [JsonProperty("start")]
        public double Start { get; set; }
        
        [JsonProperty("end")]
        public double End { get; set; }
        
        [JsonProperty("text")]
        public string Text { get; set; }
        
        [JsonProperty("actor")]
        public string Actor { get; set; }
        
        [JsonProperty("speaker")]
        public string Speaker { get; set; }
        
        [JsonProperty("onOffScreen")]
        public string OnOffScreen { get; set; }
        
        [JsonProperty("diegetic")]
        public string Diegetic { get; set; }
        
        [JsonProperty("notes")]
        public string Notes { get; set; }
        
        [JsonProperty("dialogueReverb")]
        public string DialogueReverb { get; set; }
        
        [JsonProperty("dfx")]
        public string DFX { get; set; }
    }

    public class GaudioJsonRoot
    {
        [JsonProperty("metadata")]
        public GaudioJsonMetadata Metadata { get; set; }
        
        [JsonProperty("transcriptions")]
        public List<GaudioJsonTranscription> Transcriptions { get; set; }
    }

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

            // .json
            if (jsonText.Contains("\"gaudioJsonVersion\"", StringComparison.Ordinal) && 
                jsonText.Contains("\"transcriptions\"", StringComparison.Ordinal))
            {
                return true;
            }

            return false;
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

        private string GetCreatedTime(Subtitle subtitle)
        {
            // 기존 파일에서 created 시간을 가져오려고 시도
            try
            {
                // Subtitle 객체에 원본 파일 정보가 있는지 확인
                if (subtitle.FileName != null && System.IO.File.Exists(subtitle.FileName))
                {
                    var lines = System.IO.File.ReadAllLines(subtitle.FileName);
                    var jsonText = string.Join("", lines).Trim();
                    
                    if (jsonText.StartsWith("{\"metadata\"", StringComparison.Ordinal))
                    {
                        var createdStart = jsonText.IndexOf("\"created\":\"", StringComparison.Ordinal);
                        if (createdStart >= 0)
                        {
                            createdStart += 11; // Skip "created":"
                            var createdEnd = jsonText.IndexOf("\"", createdStart);
                            if (createdEnd > createdStart)
                            {
                                var createdTime = jsonText.Substring(createdStart, createdEnd - createdStart);
                                // 유효한 날짜 형식인지 확인
                                if (DateTime.TryParse(createdTime, out _))
                                {
                                    return createdTime;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                // 파일 읽기 실패 시 무시하고 현재 시간 사용
            }
            
            // 기존 created 시간을 찾을 수 없으면 현재 시간 사용
            return DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture);
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
            try
            {
                // 메타데이터 생성
                var metadata = new GaudioJsonMetadata
                {
                    GaudioJsonVersion = "1.0",
                    SubtitleEditVersion = Utilities.AssemblyVersion,
                    Format = "Gaudio JSON",
                    Created = GetCreatedTime(subtitle),
                    Modified = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture),
                    SubtitleCount = subtitle.Paragraphs.Count,
                    TotalDuration = subtitle.Paragraphs.Count > 0 
                        ? subtitle.Paragraphs[subtitle.Paragraphs.Count - 1].EndTime.TotalSeconds 
                        : 0.0,
                    Title = title
                };
                
                // transcriptions 생성
                var transcriptions = new List<GaudioJsonTranscription>();
                var count = 0;
                
                foreach (var p in subtitle.Paragraphs)
                {
                    var transcription = new GaudioJsonTranscription
                    {
                        Index = count + 1,
                        Start = p.StartTime.TotalSeconds,
                        End = p.EndTime.TotalSeconds,
                        Text = p.Text,
                        Speaker = p.Actor,
                        OnOffScreen = p.OnOff_Screen,
                        Diegetic = p.Diegetic,
                        Notes = p.Notes,
                        DialogueReverb = p.DialogueReverb,
                        DFX = p.DFX
                    };
                    
                    transcriptions.Add(transcription);
                    count++;
                }
                
                // 루트 객체 생성
                var root = new GaudioJsonRoot
                {
                    Metadata = metadata,
                    Transcriptions = transcriptions
                };
                
                // JSON 직렬화 (들여쓰기 포함)
                var settings = new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                };
                
                return JsonConvert.SerializeObject(root, settings);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error serializing GaudioJson: {ex.Message}");
                return "{}";
            }
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
            
            try
            {
                LoadSubtitleWithMetadata(subtitle, jsonText);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GaudioJson LoadSubtitle error: {ex.Message}");
                _errorCount++;
            }
            
            subtitle.Renumber();
        }

        private void LoadSubtitleWithMetadata(Subtitle subtitle, string jsonText)
        {
            try
            {
                // Newtonsoft.Json을 사용하여 JSON 파싱
                var root = JsonConvert.DeserializeObject<GaudioJsonRoot>(jsonText);
                
                if (root?.Transcriptions == null)
                {
                    return;
                }
                
                foreach (var transcription in root.Transcriptions)
                {
                    if (string.IsNullOrEmpty(transcription.Text))
                    {
                        _errorCount++;
                        continue;
                    }
                    
                    var p = new Paragraph(
                        DecodeJsonText(transcription.Text), 
                        transcription.Start * TimeCode.BaseUnit, 
                        transcription.End * TimeCode.BaseUnit
                    );
                    
                    // 커스텀 필드들 설정
                    if (!string.IsNullOrEmpty(transcription.Actor))
                    {
                        p.Actor = DecodeJsonText(transcription.Actor);
                    }
                    else if (!string.IsNullOrEmpty(transcription.Speaker))
                    {
                        p.Actor = DecodeJsonText(transcription.Speaker);
                    }
                    
                    if (!string.IsNullOrEmpty(transcription.OnOffScreen))
                    {
                        p.OnOff_Screen = DecodeJsonText(transcription.OnOffScreen);
                    }
                    
                    if (!string.IsNullOrEmpty(transcription.Diegetic))
                    {
                        p.Diegetic = DecodeJsonText(transcription.Diegetic);
                    }
                    
                    if (!string.IsNullOrEmpty(transcription.Notes))
                    {
                        p.Notes = DecodeJsonText(transcription.Notes);
                    }
                    
                    if (!string.IsNullOrEmpty(transcription.DialogueReverb))
                    {
                        p.DialogueReverb = DecodeJsonText(transcription.DialogueReverb);
                    }
                    
                    if (!string.IsNullOrEmpty(transcription.DFX))
                    {
                        p.DFX = DecodeJsonText(transcription.DFX);
                    }
                    
                    subtitle.Paragraphs.Add(p);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error parsing metadata format with Newtonsoft.Json: {ex.Message}");
                _errorCount++;
            }
        }
    }
}
