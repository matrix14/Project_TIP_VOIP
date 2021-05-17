using NAudio.Codecs;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientWindows
{
    class SoundProcessing //TODO: on device change update recording/playing device
    {
        private WaveInEvent recorder;
        private Dictionary<String, BufferedWaveProvider> multipleBufWaveProv = new Dictionary<string, BufferedWaveProvider>();
        private WaveOut player;
        private MixingSampleProvider mixingSampleProvider;

        private List<String> activeUsersInCall;

        private ByteCallback voiceSendCallback;

        private Boolean microphoneStatus = true;
        private Boolean speakerStatus = true;

        private static int actualInputDev = 0;
        private static int actualOutputDev = 0;

        public SoundProcessing(ByteCallback voiceSendCallback)
        {
            this.voiceSendCallback = voiceSendCallback;
        }

        public ByteCallback VoiceSendCallback { get => voiceSendCallback; set => voiceSendCallback = value; }

        public void startUp(List<string> activeUsers, CancellationToken token)
        {
            
            WaveFormat format = new WaveFormat(16000, 1); //TODO: verify difference beetween (16000, 16, 1)
            recorder = new WaveInEvent()
            {
                BufferMilliseconds = 50,
                DeviceNumber = Program.setServ.getIOInputDevice(),
                WaveFormat = format
            };
            actualInputDev = Program.setServ.getIOInputDevice();
            recorder.DataAvailable += RecorderOnDataAvailable;

            mixingSampleProvider = new MixingSampleProvider(WaveFormat.CreateIeeeFloatWaveFormat(16000, 1));

            activeUsersInCall = activeUsers;
            updateInputBuffers();

            player = new WaveOut();
            player.DeviceNumber = Program.setServ.getIOOutputDevice();
            actualOutputDev = Program.setServ.getIOOutputDevice();
            clearAllBuffers();
            player.Init(mixingSampleProvider);

            int waveInDevices = WaveIn.DeviceCount;
            //WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(0);

            recorder.StartRecording();
            while (true)
            {
                if (token.IsCancellationRequested)
                    break;
                if (!speakerStatus)
                    continue;
                player.Play();
            }

        }

        public void updateIODevices()
        {
            if (Program.setServ.getIOInputDevice() != actualInputDev)
            {
                bool microphoneStatusStart = microphoneStatus;
                WaveFormat format = new WaveFormat(16000, 1); //TODO: verify difference beetween (16000, 16, 1)
                if(microphoneStatusStart)
                    recorder.StopRecording();
                recorder = new WaveInEvent()
                {
                    BufferMilliseconds = 50,
                    DeviceNumber = Program.setServ.getIOInputDevice(),
                    WaveFormat = format
                };
                actualInputDev = Program.setServ.getIOInputDevice();
                recorder.DataAvailable += RecorderOnDataAvailable;
                if (microphoneStatusStart)
                    recorder.StartRecording();
            }
            if (Program.setServ.getIOOutputDevice() != actualOutputDev)
            {
                bool speakerStatusStart = speakerStatus;
                clearAllBuffers();
                speakerStatus = false;
                this.player.Stop();
                this.player = new WaveOut();
                player.DeviceNumber = Program.setServ.getIOOutputDevice();
                actualOutputDev = Program.setServ.getIOOutputDevice();
                this.player.Init(mixingSampleProvider);
                if(speakerStatusStart)
                    speakerStatus = true;
            }
        }

        private void updateInputBuffers()
        {
            if(multipleBufWaveProv.Count==0)
            {
                foreach(string user in this.activeUsersInCall)
                {
                    multipleBufWaveProv.Add(user, new BufferedWaveProvider(recorder.WaveFormat));
                }
            } else
            {
                List<string> actualUsers = new List<string>(this.multipleBufWaveProv.Keys);
                foreach(string user in this.activeUsersInCall)
                {
                    if(actualUsers.Contains(user))
                    {
                        continue;
                    } else
                    {
                        if(!multipleBufWaveProv.ContainsKey(user))
                            multipleBufWaveProv.Add(user, new BufferedWaveProvider(recorder.WaveFormat));
                    }
                }
                foreach(string user in actualUsers)
                {
                    if(this.activeUsersInCall.Contains(user))
                    {
                        continue;
                    } else
                    {
                        if (multipleBufWaveProv.ContainsKey(user))
                        {
                            multipleBufWaveProv[user].ClearBuffer();
                            multipleBufWaveProv.Remove(user);
                        }
                    }
                }
            }
            foreach(var singleBuf in multipleBufWaveProv)
            {
                mixingSampleProvider.AddMixerInput(singleBuf.Value);
            }
        }

        private void clearAllBuffers()
        {
            foreach(var singleBuf in multipleBufWaveProv)
            {
                singleBuf.Value.ClearBuffer();
            }
        }

        public void updateUsersCount(List<string> activeUsers)
        {
            this.activeUsersInCall = activeUsers;
            updateInputBuffers();
        }

        public void stop()
        {
            player.Stop();
            recorder.StopRecording();
            clearAllBuffers();
        }

        public Boolean switchMicrophone()
        {
            if (microphoneStatus)
            {
                recorder.StopRecording();
                microphoneStatus = false;
            }
            else
            {
                recorder.StartRecording();
                microphoneStatus = true;
            }
            return microphoneStatus;
        }

        public Boolean switchSpeaker()
        {
            if (speakerStatus)
            {
                clearAllBuffers();
                player.Pause();
                speakerStatus = false;
            }
            else
            {
                clearAllBuffers();
                player.Resume();
                speakerStatus = true;
            }
            return speakerStatus;
        }

        public void incomingEncodedSound(byte[] inMsg)
        {
            if(speakerStatus==false)
            {
                return;
            }
            int position = 0;
            foreach (byte b in inMsg)
            {
                if (b == ':')
                    break;
                else
                    position++;
            }

            byte[] usernameBytes = new byte[position];
            byte[] sound = new byte[inMsg.Length - (position + 1)];

            Array.Copy(inMsg, 0, usernameBytes, 0, usernameBytes.Length);
            Array.Copy(inMsg, (position + 1), sound, 0, sound.Length);
            string username = Encoding.ASCII.GetString(usernameBytes);

            if(username==Program.username)
            {
                return;
            }

            byte[] soundRaw = DecodeSamples(sound);

            if ((!this.multipleBufWaveProv.ContainsKey(username))||this.multipleBufWaveProv[username]==null)
                return;
            this.multipleBufWaveProv[username].AddSamples(soundRaw, 0, soundRaw.Length);
        }

        private void RecorderOnDataAvailable(object sender, WaveInEventArgs waveInEventArgs)
        {
            var sound = EncodeSamples(waveInEventArgs.Buffer);

            int length = Program.username.Length;

            byte[] outMsg = new byte[sound.Length + (length + 1)];
            for(int i=0; i<length; i++)
            {
                outMsg[i] = Encoding.ASCII.GetBytes(Program.username)[i];
            }
            outMsg[length] = (byte)':';

            Array.Copy(sound, 0, outMsg, (length + 1), sound.Length);

            voiceSendCallback(outMsg);
        }

        private static byte[] EncodeSamples(byte[] data)
        {
            byte[] encoded = new byte[data.Length / 2];
            int outIndex = 0;

            for (int n = 0; n < data.Length; n += 2)
                encoded[outIndex++] = MuLawEncoder.LinearToMuLawSample(BitConverter.ToInt16(data, n));

            return encoded;
        }

        private static byte[] DecodeSamples(byte[] data)
        {
            byte[] decoded = new byte[data.Length * 2];
            int outIndex = 0;
            for (int n = 0; n < data.Length; n++)
            {
                short decodedSample = MuLawDecoder.MuLawToLinearSample(data[n]);
                decoded[outIndex++] = (byte)(decodedSample & 0xFF);
                decoded[outIndex++] = (byte)(decodedSample >> 8);
            }
            return decoded;
        }
    }
}
