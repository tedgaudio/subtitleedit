using Nikse.SubtitleEdit.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nikse.SubtitleEdit.Core.SubtitleFormats
{
    public class JsonType8 : SubtitleFormat
    {
        public override string Extension => ".json";

        public override string Name => "JSON Type 8";

        public override bool IsMine(List<string> lines, string fileName)
        {
            var subtitle = new Subtitle();
            LoadSubtitle(subtitle, lines, fileName);
            if (_errorCount >= subtitle.Paragraphs.Count)
            {
                return false;
            }
            var avgDurSecs = subtitle.Paragraphs.Average(p => p.DurationTotalSeconds);
            return avgDurSecs < 60;
        }

        public override string ToText(Subtitle subtitle, string title)
        {
            var sb = new StringBuilder(@"[");
            int count = 0;
            foreach (Paragraph p in subtitle.Paragraphs)
            {
                if (count > 0)
                {
                    sb.Append(',');
                }

                sb.Append("{\"start_time\":");
                sb.Append(p.StartTime.TotalSeconds.ToString(System.Globalization.CultureInfo.InvariantCulture));
                sb.Append(",\"end_time\":");
                sb.Append(p.EndTime.TotalSeconds.ToString(System.Globalization.CultureInfo.InvariantCulture));
                sb.Append(",\"text\":\"");
                sb.Append(Json.EncodeJsonText(p.Text));
                sb.Append("\"");
                
                // Add custom fields
                if (!string.IsNullOrEmpty(p.Actor))
                {
                    sb.Append(",\"actor\":\"");
                    sb.Append(Json.EncodeJsonText(p.Actor));
                    sb.Append("\"");
                }
                
                if (!string.IsNullOrEmpty(p.OnOff_Screen))
                {
                    sb.Append(",\"onOffScreen\":\"");
                    sb.Append(Json.EncodeJsonText(p.OnOff_Screen));
                    sb.Append("\"");
                }
                
                if (!string.IsNullOrEmpty(p.Diegetic))
                {
                    sb.Append(",\"diegetic\":\"");
                    sb.Append(Json.EncodeJsonText(p.Diegetic));
                    sb.Append("\"");
                }
                
                if (!string.IsNullOrEmpty(p.Notes))
                {
                    sb.Append(",\"notes\":\"");
                    sb.Append(Json.EncodeJsonText(p.Notes));
                    sb.Append("\"");
                }
                
                if (!string.IsNullOrEmpty(p.DialogueReverb))
                {
                    sb.Append(",\"dialogueReverb\":\"");
                    sb.Append(Json.EncodeJsonText(p.DialogueReverb));
                    sb.Append("\"");
                }
                
                if (!string.IsNullOrEmpty(p.DFX))
                {
                    sb.Append(",\"dfx\":\"");
                    sb.Append(Json.EncodeJsonText(p.DFX));
                    sb.Append("\"");
                }
                
                sb.Append("}");
                count++;
            }
            sb.Append(']');
            return sb.ToString().Trim();
        }

        public override void LoadSubtitle(Subtitle subtitle, List<string> lines, string fileName)
        {
            _errorCount = 0;
            var sb = new StringBuilder();
            foreach (string s in lines)
            {
                sb.Append(s);
            }

            string allText = sb.ToString().Trim();
            if (!(allText.StartsWith("{", StringComparison.Ordinal) || allText.StartsWith("[", StringComparison.Ordinal)))
            {
                return;
            }

            foreach (string line in allText.Split('{', '}', '[', ']'))
            {
                string s = line.Trim();
                if (s.Length > 10)
                {
                    string start = Json.ReadTag(s, "start_time");
                    string end = Json.ReadTag(s, "end_time");
                    string text = Json.ReadTag(s, "text");
                    if (start != null && end != null && text != null)
                    {
                        if (double.TryParse(start, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out var startSeconds) &&
                            double.TryParse(end, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out var endSeconds))
                        {
                            var p = new Paragraph(Json.DecodeJsonText(text), startSeconds * TimeCode.BaseUnit, endSeconds * TimeCode.BaseUnit);
                            
                            // Read custom fields
                            string actor = Json.ReadTag(s, "actor");
                            if (!string.IsNullOrEmpty(actor))
                            {
                                p.Actor = Json.DecodeJsonText(actor);
                            }
                            
                            string onOffScreen = Json.ReadTag(s, "onOffScreen");
                            if (!string.IsNullOrEmpty(onOffScreen))
                            {
                                p.OnOff_Screen = Json.DecodeJsonText(onOffScreen);
                            }
                            
                            string diegetic = Json.ReadTag(s, "diegetic");
                            if (!string.IsNullOrEmpty(diegetic))
                            {
                                p.Diegetic = Json.DecodeJsonText(diegetic);
                            }
                            
                            string notes = Json.ReadTag(s, "notes");
                            if (!string.IsNullOrEmpty(notes))
                            {
                                p.Notes = Json.DecodeJsonText(notes);
                            }
                            
                            string dialogueReverb = Json.ReadTag(s, "dialogueReverb");
                            if (!string.IsNullOrEmpty(dialogueReverb))
                            {
                                p.DialogueReverb = Json.DecodeJsonText(dialogueReverb);
                            }
                            
                            string dfx = Json.ReadTag(s, "dfx");
                            if (!string.IsNullOrEmpty(dfx))
                            {
                                p.DFX = Json.DecodeJsonText(dfx);
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
            subtitle.Renumber();
        }
    }
}
