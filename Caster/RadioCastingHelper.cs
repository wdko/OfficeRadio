using Sharpcaster.Models.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caster
{
    public class RadioCastingHelper
    {
        public static Media GetKinkMedia()
        {
            var url = "https://22343.live.streamtheworld.com/KINK.mp3";
            return GetLiveRadioMedia("KINK", "Alternative Rock Radio", url);
        }

        public static Media GetQMusicNonStopMedia()
        {
            var url = "https://icecast-qmusicnl-cdp.triple-it.nl/Qmusic_nl_nonstop_high.aac";
            return GetLiveRadioMedia("Qmusic Non-Stop", "Non-Stop Music", url);
        }

        private static Media GetLiveRadioMedia(string title, string subtitle, string url)
        {
            var media = new Media
            {
                ContentUrl = $"{url}",
                Metadata = new MediaMetadata
                {
                    Title = $"{title}",
                    SubTitle = $"{subtitle}"
                },
                StreamType = StreamType.Live
            };
            return media;
        }
    }
}
