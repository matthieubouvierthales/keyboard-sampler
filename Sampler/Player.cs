using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Sampler
{
    public class Player
    {
        private MediaPlayer _mediaPlayer;

        public bool IsLooping { get; private set; }

        public Player(Uri sound)
        {
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.Open(sound);
            _mediaPlayer.MediaEnded += _mediaPlayer_MediaEnded;
        }

        void _mediaPlayer_MediaEnded(object sender, EventArgs e)
        {
            if (IsLooping)
            {
                Play(true);
            }
        }

        public void Play(bool loop)
        {
            IsLooping = loop;
            _mediaPlayer.Position = new TimeSpan(0,0,0);
            _mediaPlayer.Play();
        }

        public void Stop()
        {
            _mediaPlayer.Stop();
            IsLooping = false;
        }


    }
}
