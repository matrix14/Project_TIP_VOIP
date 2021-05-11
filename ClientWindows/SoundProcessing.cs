using NAudio.Codecs;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientWindows
{
    class SoundProcessing
    {
        private WaveInEvent recorder;
        private BufferedWaveProvider bufferedWaveProvider;
        private WaveOut player;

        private ByteCallback voiceSendCallback;

        public SoundProcessing(ByteCallback voiceSendCallback)
        {
            this.voiceSendCallback = voiceSendCallback;
        }

        public ByteCallback VoiceSendCallback { get => voiceSendCallback; set => voiceSendCallback = value; }

        public void startUp()
        {
            WaveFormat format = new WaveFormat(16000, 16, 1);
            recorder = new WaveInEvent()
            {
                BufferMilliseconds = 50,
                DeviceNumber = 1,
                WaveFormat = format
            };
            recorder.DeviceNumber = 0;
            recorder.DataAvailable += RecorderOnDataAvailable;

            //Up to here its ok
            
            bufferedWaveProvider = new BufferedWaveProvider(recorder.WaveFormat);
            bufferedWaveProvider.DiscardOnBufferOverflow = true; //TODO: temporary

            // set up playback
            player = new WaveOut();
            player.Init(bufferedWaveProvider);

            int waveInDevices = WaveIn.DeviceCount;
            WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(0);


            recorder.StartRecording();
            while (true)
            {
                player.Play();
            }

        }

        public void incomingEncodedSound(byte[] inMsg)
        {
            int position = 0;
            foreach (byte b in inMsg)
            {
                if (b == ':')
                    break;
                else
                    position++;
            }

            byte[] sound = new byte[inMsg.Length - (position + 1)];

            Array.Copy(inMsg, (position + 1), sound, 0, sound.Length);
            byte[] soundRaw = DecodeSamples(sound);
            //if(bufferedWaveProvider.)
            bufferedWaveProvider.AddSamples(soundRaw, 0, soundRaw.Length);
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

            //bufferedWaveProvider.AddSamples(DecodeSamples(x), 0, DecodeSamples(x).Length);
            //bufferedWaveProvider.AddSamples(waveInEventArgs.Buffer, 0, waveInEventArgs.BytesRecorded);
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
