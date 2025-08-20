using Sharpcaster;
using Sharpcaster.Models;
using Sharpcaster.Models.Media;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace Caster
{
    public class ChromeCastDevice
    {
        private ChromecastReceiver Receiver { get; set; }
        private ChromecastClient Client { get; set; }
        public string Name => Receiver.Name;
        public string Model => Receiver.Model;
        public Guid? Id => Client?.SenderId;
        public string? FriendlyName => Client?.FriendlyName;
        public string? Version => Receiver.Version;
        public Uri? DeviceUri => Receiver.DeviceUri;


        public MediaStatus? Status = null;
        public bool Connected { get; private set; } = false;
        public event EventHandler StatusChanged;
        public ChromeCastDevice(ChromecastReceiver receiver)
        {
            Receiver = receiver ?? throw new ArgumentNullException(nameof(receiver));
        }

        /// <summary>
        /// Loads the default media receiver with the specified media.
        /// </summary>
        /// <param name="media"></param>
        public async void LoadDefautlMedia(Media media)
        {
            await Connect();
            
            await Client.LaunchApplicationAsync(GetDefaultReceiverCode());
            var mediaStatus = await Client.MediaChannel.LoadAsync(media);
        }

        /// <summary>
        /// Set volume betweeen 0.0 to 1.0
        /// </summary>
        /// <param name="volume"></param>
        /// <returns></returns>
        public async Task SetVolumeAsync(double volume)
        {
            if (Client == null)
                await Connect();
            if (Client != null)
            {
                await Client.ReceiverChannel.SetVolume(volume);
            }
        }

        public async Task PlayAsync()
        {
            if (Client == null)
                await Connect();
            if (Client != null)
            {
                await Client.MediaChannel.PlayAsync();
            }
        }
        public async Task PauseAsync()
        {
            if (Client == null)
                await Connect();
            if (Client != null)
            {
                await Client.MediaChannel.PauseAsync();
            }
        }

        public async Task Connect()
        {
            if(Client == null)
            {
                Client = new ChromecastClient();
                var status = await Client.ConnectChromecast(Receiver);
                Client.MediaChannel.StatusChanged += OnMediaStatusChanged;
                Client.Disconnected += OnDisconnected;
                Connected = true;
            }
        }

        private async Task Disconnect(bool already)
        {
            if (Client != null)
            {
                if(!already)
                    await Client.DisconnectAsync();

                Client.MediaChannel.StatusChanged -= OnMediaStatusChanged;
                Client.Disconnected -= OnDisconnected;
                Client = null;
                Connected = false;
            }
        }

        private void OnMediaStatusChanged(object sender, MediaStatus e)
        {
            Status = e;
            StatusChanged?.Invoke(this, EventArgs.Empty);
        }

        private async void OnDisconnected(object sender, EventArgs e)
        {
            await Disconnect(true);
        }
        /// <summary>
        /// Returns the receiver code for the Chromecasts default media receiver.
        /// </summary>
        /// <returns></returns>
        private string GetDefaultReceiverCode()
        {
            return "CC1AD845";
        }
    }

    public class CastingService
    {
        private MdnsChromecastLocator Locator { get; set; }

        public CastingService()
        {
            Locator = new MdnsChromecastLocator();
            
        }

        public async IAsyncEnumerable<ChromeCastDevice> DiscoverReceiversAsync()
        {
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
            foreach(var dev in  await Locator.FindReceiversAsync(cancellationToken))
            {
                yield return new ChromeCastDevice(dev);
            }
        }

        public static Media GetTextToSpeechMedia(string text)
        {
            var url = $"https://translate.google.com/translate_tts?ie=UTF-8&q={Uri.EscapeDataString(text)}&tl=en&client=tw-ob";
            var media = new Media
            {
                ContentUrl = $"{url}",
                Metadata = new MediaMetadata
                {
                    Title = "Text to speech",
                    SubTitle = "Test"
                }
            };
            return media;
        }
    }
}
