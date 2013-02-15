Imports Microsoft.Expression.Encoder
Imports Microsoft.Expression.Encoder.Devices
Imports Microsoft.Expression.Encoder.Live
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Drawing
Imports System.Drawing.Imaging

Public Class Webcam

    Private vidDevice As EncoderDevice
    Private audDevice As EncoderDevice

    Private deviceSource As LiveDeviceSource
    Private job As LiveJob

    Private _isRecording As Boolean
    Private canCapture As Boolean
    Private isCapturing As Boolean

    Private vidDirectory As String
    Private _vidFormat As VideoFormat = VideoFormat.wmv

    Private imgDirectory As String
    Private imgFormat As ImageFormat = ImageFormat.Jpeg

    Private _frameRate As Integer = 15
    Private _frameSize As Size = New Size(320, 240)

#Region "VideoDirectory Dependency Property"
    ''' <summary>
    ''' Gets or Sets the folder where the recorded webcam video will be saved.
    ''' </summary>    
    Public Property VideoDirectory() As String
        Get
            Return CType(GetValue(VideoDirectoryProperty), String)
        End Get
        Set(ByVal value As String)
            SetValue(VideoDirectoryProperty, value)
        End Set
    End Property

    Public Shared VideoDirectoryProperty As DependencyProperty = _
        DependencyProperty.Register("VideoDirectory", GetType(String), GetType(Webcam), _
                                    New FrameworkPropertyMetadata(New PropertyChangedCallback( _
                                                                  AddressOf DirectoryChange)))

    Private Shared Sub DirectoryChange(ByVal source As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        CType(source, Webcam).vidDirectory = CType(e.NewValue, String)
    End Sub

#End Region

#Region "VidFormat Dependency Property"
    ''' <summary>
    ''' Gets or Sets the video format in which the webcam video will be saved.
    ''' </summary>    
    Public Property VidFormat() As VideoFormat
        Get
            Return CType(GetValue(VidFormatProperty), VideoFormat)
        End Get
        Set(ByVal value As VideoFormat)
            SetValue(VidFormatProperty, value)
        End Set
    End Property

    Public Shared VidFormatProperty As DependencyProperty = _
        DependencyProperty.Register("VidFormat", GetType(VideoFormat), GetType(Webcam), _
                                    New FrameworkPropertyMetadata(New PropertyChangedCallback( _
                                                                  AddressOf VidFormatChange)))

    Private Shared Sub VidFormatChange(ByVal source As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        CType(source, Webcam)._vidFormat = CType(e.NewValue, VideoFormat)
    End Sub

#End Region

#Region "VideoDevice Dependency Property"
    ''' <summary>
    ''' Gets or Sets the name of the video device to be used.
    ''' </summary>    
    Public Property VideoDevice() As String
        Get
            Return CType(GetValue(VideoDeviceProperty), String)
        End Get
        Set(ByVal value As String)
            SetValue(VideoDeviceProperty, value)
        End Set
    End Property

    Public Shared VideoDeviceProperty As DependencyProperty = _
        DependencyProperty.Register("VideoDevice", GetType(String), GetType(Webcam), _
                                    New FrameworkPropertyMetadata(New PropertyChangedCallback( _
                                                                  AddressOf VidDeviceChange)))

    Private Shared Sub VidDeviceChange(ByVal source As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim deviceName As String = CType(e.NewValue, String)
        Dim eDev = EncoderDevices.FindDevices(EncoderDeviceType.Video).Where _
                   (Function(dv) dv.Name = deviceName)

        If (eDev.Count > 0) Then
            CType(source, Webcam).vidDevice = eDev.First

            Try
                CType(source, Webcam).Display()
            Catch ex As Microsoft.Expression.Encoder.SystemErrorException
                Exit Sub
            End Try
        End If
    End Sub

#End Region

#Region "AudioDevice Dependency Property"
    ''' <summary>
    ''' Gets or Sets the name of the audio device to be used.
    ''' </summary>    
    Public Property AudioDevice() As String
        Get
            Return CType(GetValue(AudioDeviceProperty), String)
        End Get
        Set(ByVal value As String)
            SetValue(AudioDeviceProperty, value)
        End Set
    End Property

    Public Shared AudioDeviceProperty As DependencyProperty = _
        DependencyProperty.Register("AudioDevice", GetType(String), GetType(Webcam), _
                                    New FrameworkPropertyMetadata(New PropertyChangedCallback( _
                                                                  AddressOf AudioDeviceChange)))

    Private Shared Sub AudioDeviceChange(ByVal source As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        Dim deviceName As String = CType(e.NewValue, String)
        Dim eDev = EncoderDevices.FindDevices(EncoderDeviceType.Audio).Where _
                   (Function(dv) dv.Name = deviceName)

        If (eDev.Count > 0) Then
            CType(source, Webcam).audDevice = eDev.First

            Try
                CType(source, Webcam).Display()
            Catch ex As Microsoft.Expression.Encoder.SystemErrorException
                Exit Sub
            End Try
        End If
    End Sub

#End Region

#Region "PictureFormat Dependency Property"
    ''' <summary>
    ''' Gets or Sets format used when saving snapshot of webcam stream.
    ''' </summary>    
    Public Property PictureFormat() As ImageFormat
        Get
            Return CType(GetValue(PictureFormatProperty), ImageFormat)
        End Get
        Set(ByVal value As ImageFormat)
            SetValue(PictureFormatProperty, value)
        End Set
    End Property

    Public Shared PictureFormatProperty As DependencyProperty = _
        DependencyProperty.Register("PictureFormat", GetType(ImageFormat), GetType(Webcam), _
                                    New FrameworkPropertyMetadata(New PropertyChangedCallback( _
                                                                  AddressOf PictureFormatChange)))

    Private Shared Sub PictureFormatChange(ByVal source As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        CType(source, Webcam).imgFormat = CType(e.NewValue, ImageFormat)
    End Sub

#End Region

#Region "ImageDirectory Dependency Property"
    ''' <summary>
    ''' Gets or Sets the folder where snapshot will be saved.
    ''' </summary>    
    Public Property ImageDirectory() As String
        Get
            Return CType(GetValue(ImageDirectoryProperty), String)
        End Get
        Set(ByVal value As String)
            SetValue(ImageDirectoryProperty, value)
        End Set
    End Property

    Public Shared ImageDirectoryProperty As DependencyProperty = _
        DependencyProperty.Register("ImageDirectory", GetType(String), GetType(Webcam), _
                                    New FrameworkPropertyMetadata(New PropertyChangedCallback( _
                                                                  AddressOf ImageDirectoryChange)))

    Private Shared Sub ImageDirectoryChange(ByVal source As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        CType(source, Webcam).imgDirectory = CType(e.NewValue, String)
    End Sub

#End Region

#Region "Frame rate dependency property"

    ''' <summary>
    ''' Gets or sets the frame rate in frames per second.
    ''' Default is 15.
    ''' </summary>    
    Public Property FrameRate As Integer
        Get
            Return CType(GetValue(FrameRateProperty), Integer)
        End Get
        Set(value As Integer)
            SetValue(FrameRateProperty, value)
        End Set
    End Property

    Public Shared FrameRateProperty As DependencyProperty = _
        DependencyProperty.Register("FrameRate", GetType(Integer), GetType(Webcam), _
                                    New FrameworkPropertyMetadata(New PropertyChangedCallback( _
                                                                  AddressOf FrameRateChange)))

    Private Shared Sub FrameRateChange(ByVal source As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        CType(source, Webcam)._frameRate = CType(e.NewValue, Integer)
    End Sub

#End Region

#Region "Frame size dependency property"

    ''' <summary>
    ''' Gets or sets the size of the video profile.
    ''' Default is 320x240
    ''' </summary>    
    Public Property FrameSize As Size
        Get
            Return CType(GetValue(FrameSizeProperty), Size)
        End Get
        Set(value As Size)
            SetValue(FrameSizeProperty, value)
        End Set
    End Property

    Public Shared FrameSizeProperty As DependencyProperty = _
        DependencyProperty.Register("FrameSize", GetType(Size), GetType(Webcam), _
                                    New FrameworkPropertyMetadata(New PropertyChangedCallback( _
                                                                  AddressOf FrameSizeChange)))

    Private Shared Sub FrameSizeChange(ByVal source As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
        CType(source, Webcam)._frameSize = CType(e.NewValue, Size)
    End Sub

#End Region

    ''' <summary>
    ''' Gets a value indicating whether video recording is taking place.
    ''' </summary>    
    Public ReadOnly Property IsRecording As Boolean
        Get
            Return _isRecording
        End Get
    End Property

    ' Display video from webcam.
    Private Sub Display()
        If (canCapture = True) Then
            If (vidDevice IsNot Nothing) Then
                StopRecording()
                Dispose()

                job = New LiveJob

                deviceSource = job.AddDeviceSource(vidDevice, audDevice)
                deviceSource.PickBestVideoFormat(_frameSize, _frameRate)
                WebcamPanel.Size = _frameSize
                job.OutputFormat.VideoProfile.Size = _frameSize
                deviceSource.PreviewWindow = New PreviewWindow(New HandleRef(WebcamPanel, WebcamPanel.Handle))

                job.ActivateSource(deviceSource)

                isCapturing = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Display webcam video on control.
    ''' </summary>
    Public Sub StartCapture()
        If (canCapture = False) Then
            canCapture = True

            Try
                Display()
            Catch ex As Microsoft.Expression.Encoder.SystemErrorException
                canCapture = False
                Throw New Microsoft.Expression.Encoder.SystemErrorException
            End Try
        Else
            Exit Sub
        End If
    End Sub

    ''' <summary>
    ''' Stop the capturing/display of webcam video.
    ''' </summary>
    Public Sub StopCapture()
        If (canCapture = True) Then
            canCapture = False
            isCapturing = False
            StopRecording()
            Dispose()
        End If
    End Sub

    ''' <summary>
    ''' Starts the recording of webcam images to a video file.
    ''' </summary>
    Public Sub StartRecording()
        If (vidDirectory <> String.Empty AndAlso job IsNot Nothing) Then
            If (Directory.Exists(vidDirectory) = False) Then
                Throw New DirectoryNotFoundException("The specified directory does not exist")
                Exit Sub
            End If

            ' If isCapturing is true then it means the control is capturing images 
            ' from the webcam.
            If (isCapturing = True) Then
                StopRecording()
                job.PublishFormats.Clear()

                Dim timeStamp As String = DateTime.Now.ToString
                timeStamp = timeStamp.Replace("/", "-")
                timeStamp = timeStamp.Replace(":", ".")
                Dim filePath As String = vidDirectory & "\WebcamVid " & timeStamp & "." & _vidFormat.ToString

                Dim fileArchFormat As New FileArchivePublishFormat(filePath)
                job.PublishFormats.Add(fileArchFormat)
                job.StartEncoding()

                _isRecording = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Stops the recording of webcam video.
    ''' </summary>
    Public Sub StopRecording()
        If (job IsNot Nothing AndAlso _isRecording = True) Then
            job.StopEncoding()
            _isRecording = False
        End If
    End Sub

    ''' <summary>
    ''' Take snapshot of a webcam image. 
    ''' </summary>
    Public Sub TakeSnapshot()
        If (imgDirectory <> String.Empty AndAlso job IsNot Nothing) Then
            If (Directory.Exists(imgDirectory) = False) Then
                Throw New DirectoryNotFoundException("The specified directory does not exist")
                Exit Sub
            End If

            ' If isCapturing is true then it means the control is capturing images 
            ' from the webcam.
            If (isCapturing = True) Then
                Dim panelWidth As Integer = CInt(Me.ActualWidth)
                Dim panelHeight As Integer = CInt(Me.ActualHeight)

                Dim timeStamp As String = DateTime.Now.ToString
                timeStamp = timeStamp.Replace("/", "-")
                timeStamp = timeStamp.Replace(":", ".")

                Dim filePath As String = imgDirectory & "\Snapshot " & timeStamp & "." & imgFormat.ToString

                Dim pnlPnt As Point = WebcamPanel.PointToScreen(New Point(WebcamPanel.ClientRectangle.X, _
                                                                          WebcamPanel.ClientRectangle.Y))

                Using bmp As New Bitmap(panelWidth, panelHeight)
                    Using gcs As Graphics = Graphics.FromImage(bmp)
                        gcs.CopyFromScreen(pnlPnt, Point.Empty, New Size(panelWidth, panelHeight))
                    End Using
                    bmp.Save(filePath, imgFormat)
                End Using
            End If
        End If
    End Sub

    Public Sub Dispose()
        If (deviceSource IsNot Nothing AndAlso job IsNot Nothing) Then
            deviceSource = Nothing
            job.Dispose()
        End If
    End Sub

End Class
