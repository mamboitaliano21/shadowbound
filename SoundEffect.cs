using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.XAudio2;
using SharpDX.IO;
using SharpDX.Multimedia;

namespace Lab
{
    public class SoundEffect
    {
        // constructor and play method is copied from
        // https://stevenpeirce.wordpress.com/2013/03/23/sharpdx-audio-in-windows-8-app/
        // with slight modification

        readonly XAudio2 _xaudio;
        readonly WaveFormat _waveFormat;
        readonly AudioBuffer _buffer;
        readonly SoundStream _soundstream;
        private SourceVoice sourceVoice;

        public bool isStarted;

        public SoundEffect(string soundFxPath, bool infiniteLoop)
        {
            _xaudio = new XAudio2();
            var masteringsound = new MasteringVoice(_xaudio);

            var nativefilestream = new NativeFileStream(
            soundFxPath,
            NativeFileMode.Open,
            NativeFileAccess.Read,
            NativeFileShare.Read);

            _soundstream = new SoundStream(nativefilestream);
            _waveFormat = _soundstream.Format;
            _buffer = new AudioBuffer
            {
            Stream = _soundstream.ToDataStream(),
            AudioBytes = (int) _soundstream.Length,
            Flags = BufferFlags.EndOfStream
            };
            if (infiniteLoop)
            {
                _buffer.LoopCount = AudioBuffer.LoopInfinite;
            }
            isStarted = false;
        }

        public void Play()
        {
            sourceVoice = new SourceVoice(_xaudio, _waveFormat, true);
            sourceVoice.SubmitSourceBuffer(_buffer, null);
            sourceVoice.Start();
            isStarted = true;
        }

        public void Stop()
        {
            sourceVoice.Stop(PlayFlags.None, 0);
            sourceVoice.FlushSourceBuffers();
            isStarted = false;
        }

        public float GetVolume()
        {
            return sourceVoice.Volume;
        }

        public void SetVolume(float newVolume)
        {
            sourceVoice.SetVolume(newVolume);
        }
    }
}
