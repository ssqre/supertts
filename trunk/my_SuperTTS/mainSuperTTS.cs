using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Speech;
using System.Speech.Synthesis;
using System.IO;
using System.Media;
using System.Diagnostics;

namespace my_SuperTTS
{
    public partial class mainSuperTTS : Form
    {
        SpeechSynthesizer speaker = new SpeechSynthesizer();//实例化一个语音合成类

        public mainSuperTTS()
        {
            InitializeComponent();
        }

        private void btnSpeak_Click(object sender, EventArgs e)
        {
            if (textDisplay.Text != string.Empty)
            {
                //speaker.SelectVoice(comboBoxVoice.SelectedItem.ToString());
                //speaker.SpeakAsync(textDisplay.Text);

                speaker.SetOutputToWaveFile(Application.StartupPath + @"/original.wav");
                speaker.Speak(textDisplay.Text);
                speaker.SetOutputToDefaultAudioDevice();

                Process p = new Process();
                p.StartInfo = new ProcessStartInfo();
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.FileName = "ctcp.exe";
                p.StartInfo.Arguments = "original.wav" + " " + "destination.wav" + " " + "-tempo="
                    + trackBarSpeed.Value.ToString() + " " + "-pitch=" + trackBarPitch.Value.ToString();
                p.Start();

                /*
                while (!p.HasExited)
                {
                }
                */
                p.WaitForExit();//和while的作用一样，等待子进程执行完毕
                
                
                SoundPlayer soundplayer = new SoundPlayer(Application.StartupPath + @"/destination.wav");
                soundplayer.Play();
                
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = Path.GetDirectoryName(path);
            if (textDisplay.Text != string.Empty)
            {
                saveFileDialog1.Filter = "wav files(*.wav)|*.wav|All files(*.*)|*.*";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    speaker.SetOutputToWaveFile(Application.StartupPath + @"/original.wav");
                    speaker.Speak(textDisplay.Text);
                    speaker.SetOutputToDefaultAudioDevice();

                    Process p = new Process();
                    p.StartInfo = new ProcessStartInfo();
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.StartInfo.FileName = path + @"\ctcp.exe";
                    p.StartInfo.Arguments = path + @"\original.wav" + " " + saveFileDialog1.FileName + " " + "-tempo="
                        + trackBarSpeed.Value.ToString() + " " + "-pitch=" + trackBarPitch.Value.ToString();
                    p.Start();
                }
            }
            else
            {
                MessageBox.Show("没有要合成的文本内容", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }           
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = textFileAddress.Text;
            if (openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                textFileAddress.Text = openFileDialog1.FileName;

                if (!File.Exists(textFileAddress.Text))
                {
                    FileStream fs = File.Create(textFileAddress.Text);
                }

                textDisplay.Text = File.ReadAllText(textFileAddress.Text, Encoding.Default);
            }

        }

        private void mainSuperTTS_Load(object sender, EventArgs e)
        {
            foreach(InstalledVoice voice in speaker.GetInstalledVoices())
            {
                comboBoxVoice.Items.Add(voice.VoiceInfo.Name);
            }

            comboBoxVoice.SelectedIndex = 0;

            speaker.SelectVoice(comboBoxVoice.SelectedItem.ToString());

            speaker.Volume = trackBarVolume.Value;

        }

        private void comboBoxVoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            speaker.SelectVoice(comboBoxVoice.SelectedItem.ToString());
        }

        private void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            speaker.Volume = trackBarVolume.Value;
        }

        /*设定性别
        private VoiceGender GetGender()
        {
            string s = comboBoxGender.SelectedItem.ToString();
            VoiceGender voicegender;

            switch (s)
            {
                case "无指定":
                    voicegender = VoiceGender.NotSet;
            	    break;
                case "男性":
                    voicegender = VoiceGender.Male;
                    break;
                case "女性":
                    voicegender = VoiceGender.Female;
                    break;
                case "中性":
                    voicegender = VoiceGender.Neutral;
                    break;
                default:
                    voicegender = VoiceGender.NotSet;
                    break;
            }
            return voicegender;
        }
        */

        /*设定年龄
        private VoiceAge GetAge()
        {
            string s = comboBoxAge.SelectedItem.ToString();
            VoiceAge voiceage;

            switch (s)
            {
                case "无指定":
                    voiceage = VoiceAge.NotSet;
                    break;
                case "小孩":
                    voiceage = VoiceAge.Child;
                    break;
                case "青年":
                    voiceage = VoiceAge.Teen;
                    break;
                case "成年":
                    voiceage = VoiceAge.Adult;
                    break;
                case "老年":
                    voiceage = VoiceAge.Senior;
                    break;
                default:
                    voiceage = VoiceAge.NotSet;
                    break;
            }
            return voiceage;
        }
        */

        /*AudioProcess
        private Process AudioProcess()
        {
            Process p = new Process();
            p.StartInfo = new ProcessStartInfo();
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.FileName = "ctcp.exe";
            p.StartInfo.Arguments = "original.wav" + " " + "destination.wav" + " " + "-tempo="
                + trackBarSpeed.Value.ToString() + " " + "-pitch=" + trackBarPitch.Value.ToString();
            p.Start();
            return p;
        }
         * */
    }
}
