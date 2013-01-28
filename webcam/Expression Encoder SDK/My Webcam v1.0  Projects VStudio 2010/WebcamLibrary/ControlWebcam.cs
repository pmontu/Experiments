using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Expression.Encoder.Devices;
using Microsoft.Expression.Encoder.Live;
using Microsoft.Expression.Encoder;


namespace WebcamLibrary
{
    public class ControlWebcam
    {
        public EncoderDevice SelectedVideoDevice { get; set; }
        public List<EncoderDevice> ListVideoDevices { get; set; }
        public EncoderDevice SelectedAudioDevice { get; set; }
        public List<EncoderDevice> ListAudioDevices { get; set; }
        public LiveJob LiveJob { get; set; }
        public bool IsRecording { get; set; }
        public LiveDeviceSource LiveDeviceSource { get; set; }
        public FileArchivePublishFormat FileArchivePublishFormat { get; set; }
        public PullBroadcastPublishFormat PullBroadcastPublishFormat { get; set; }
      
        /// <summary>
        /// Default contstructor.
        /// </summary>
        public ControlWebcam()
        {
            // Initialize the list of the audio and video devices
            ListVideoDevices = new List<EncoderDevice>();
            ListAudioDevices = new List<EncoderDevice>();
        }

        /// <summary> 
        /// Initialize and fill the list (ListVideoDevices) with all video devices
        /// </summary>
        /// <returns> Return the number of devices (0 = no device)</returns>
        public int InitializeListVideoDevices()
        {
            int nb = 0;
            foreach (EncoderDevice encoderDevice in EncoderDevices.FindDevices(EncoderDeviceType.Video))
            {
                ListVideoDevices.Add(encoderDevice);
                nb++;
            }
            return nb;
        }

        /// <summary>
        /// Initialize and fill the list (ListAudioDevices) with all audio devices
        /// </summary>
        /// <returns>Return the number of devices (0 = no device)</returns>
        public int InitializeListAudioDevices()
        {
            int nb = 0;
            foreach (EncoderDevice encoderDevice in EncoderDevices.FindDevices(EncoderDeviceType.Audio))
            {
                ListAudioDevices.Add(encoderDevice);
                nb++;
            }
            return nb;
        }

        /// <summary>
        /// Start the webcam. Create the liveJob and set the activate source (audio and video)
        /// </summary>
        /// <returns>Return false if the webcam cannot start</returns>
        public bool StartWebcam()
        {
            if (SelectedVideoDevice == null || SelectedAudioDevice == null) return false;
            LiveJob = null;
            LiveJob = new LiveJob();
            LiveDeviceSource = LiveJob.AddDeviceSource(SelectedVideoDevice, SelectedAudioDevice);
            LiveJob.ActivateSource(LiveDeviceSource);
            return true;
        }

        /// <summary>
        /// Stop the webcam. Remove the active device of the liveJob
        /// </summary>
        public void StopWebcam()
        {
            LiveJob.RemoveDeviceSource(LiveDeviceSource);
            LiveDeviceSource = null;
        }

        /// <summary>
        /// Open the form of the video settings.
        /// </summary>
        /// <param name="parentWindow">The parent window (Anchor of the setting panel)</param>
        public void DisplayVideoSettings(HandleRef parentWindow)
        {
            LiveDeviceSource.ShowConfigurationDialog(ConfigurationDialog.VideoCaptureDialog, parentWindow);
        }

        /// <summary>
        /// Open the form of audio settings
        /// </summary>
        /// <param name="parentWindow">The parent window (Anchor of the setting panel)</param>
        public void DisplayAudioSettings(HandleRef parentWindow)
        {
            LiveDeviceSource.ShowConfigurationDialog(ConfigurationDialog.AudioCaptureDialog, new HandleRef(parentWindow,parentWindow.Handle));
        }

        /// <summary>
        /// Record video of the selected webcam and save into a wmv file
        /// </summary>
        /// <param name="outputPath">Path of the output file (ex: "C:\... \video1.wmv")</param>
        /// <returns>Return true if the recording is starting with success</returns>
        public bool StartRecording(String outputPath)
        {
            if (outputPath.Length > 0)
            {
                FileArchivePublishFormat = new FileArchivePublishFormat();
                FileArchivePublishFormat.OutputFileName = outputPath;
                LiveJob.PublishFormats.Add(FileArchivePublishFormat);
                LiveJob.StartEncoding();
                IsRecording = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Stop the recording of the selected webcam
        /// </summary>
        public void StopRecording()
        {
            LiveJob.StopEncoding();
            IsRecording = false;
        }

        /// <summary>
        /// Create a stream flux of the selected webcam throught the specified port.
        /// </summary>
        /// <param name="port">Port of the streaming flux (ex: "8080")</param>
        /// <param name="maxNbConnections">Number maximum of connections</param>
        /// <param name="qualityFormat">Define the quality of the flux (use "LivePresets" enum)</param>
        /// <returns>Return TRUE if the streaming start with success</returns>
        public bool StartStreaming(int port,int maxNbConnections,Preset qualityFormat)
        {
            if (port > 0 && maxNbConnections > 0 && qualityFormat !=null)
            {
                PullBroadcastPublishFormat = new PullBroadcastPublishFormat();
                PullBroadcastPublishFormat.BroadcastPort = port;
                PullBroadcastPublishFormat.MaximumNumberOfConnections = maxNbConnections;

                LiveJob.ApplyPreset(qualityFormat);

                LiveJob.PublishFormats.Add(PullBroadcastPublishFormat);
                LiveJob.StartEncoding();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Stop the streaming flux of the webcam
        /// </summary>
        public void StopStreaming()
        {
            LiveJob.StopEncoding();
        }
    }
}
