using Nikse.SubtitleEdit.Logic;
using System.Windows.Forms;
using Nikse.SubtitleEdit.Core.Common;
using System;

namespace Nikse.SubtitleEdit.Forms.Assa
{
    public sealed partial class SetLayer : Form
    {
        private Subtitle _subtitle;
        private Paragraph _p;
        public int Layer { get; set; }
        public string Actor { get; set; }
        public string OnOffScreen { get; set; }
        public string Diegetic { get; set; }
        public string DFX { get; set; }
        public string DialogueReverb { get; set; }
        public string Notes { get; set; }

        public SetLayer(Subtitle subtitle, Paragraph p)
        {
            UiUtil.PreInitialize(this);
            InitializeComponent();
            UiUtil.FixFonts(this);

            _subtitle = subtitle;
            _p = p;
            Text = LanguageSettings.Current.Main.Menu.ContextMenu.SetLayer;

            numericUpDownLayer.Minimum = int.MinValue;
            numericUpDownLayer.Maximum = int.MaxValue;
            numericUpDownLayer.Value = p?.Layer ?? 0;
            
            // 현재 값들 설정
            comboBoxActor.Text = p?.Actor ?? string.Empty;
            comboBoxOnOffScreen.Text = p?.OnOff_Screen ?? string.Empty;
            comboBoxDiegetic.Text = p?.Diegetic ?? string.Empty;
            textBoxDFX.Text = p?.DFX ?? string.Empty;
            comboBoxDialogueReverb.Text = p?.DialogueReverb ?? string.Empty;
            textBoxNotes.Text = p?.Notes ?? string.Empty;

            // 언어 설정
            var language = LanguageSettings.Current.General;
            labelLayer.Text = "Layer";
            labelActor.Text = language.Actor;
            labelOnOffScreen.Text = language.OnOffScreen;
            labelDiegetic.Text = language.Diegetic;
            labelDFX.Text = language.DFX;
            labelDialogueReverb.Text = language.DialogueReverb;
            labelNotes.Text = language.Notes;
        }

        private void SetLayer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }
        }

        private void numericUpDownLayer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonOK_Click(null, null);
            }
        }

        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            Layer = (int)numericUpDownLayer.Value;
            Actor = comboBoxActor.Text;
            OnOffScreen = comboBoxOnOffScreen.Text;
            Diegetic = comboBoxDiegetic.Text;
            DFX = textBoxDFX.Text;
            DialogueReverb = comboBoxDialogueReverb.Text;
            Notes = textBoxNotes.Text;
            DialogResult = DialogResult.OK;
        }

        private void SetLayer_Shown(object sender, System.EventArgs e)
        {
            numericUpDownLayer.Focus();
            TaskDelayHelper.RunDelayed(TimeSpan.FromMilliseconds(10), () => numericUpDownLayer.Focus());
        }
    }
}
