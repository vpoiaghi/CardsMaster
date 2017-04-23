Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO

Namespace Skins

    Public MustInherit Class SkinElement

        Private Enum TextureTypes
            Color
            GradientColor
            Image
        End Enum

        Public Property X As Integer
        Public Property Y As Integer
        Public Property Width As Integer
        Public Property Height As Integer

        Private m_color1 As Color
        Private m_color2 As Color
        Private m_imageName As String
        Private m_texturesDirectory As DirectoryInfo
        Private m_type As TextureTypes

        Public MustOverride Sub Draw(g As Graphics)

        Protected Sub New(width As Integer, height As Integer)
            Me.New(0, 0, width, height)
        End Sub

        Protected Sub New(x As Integer, y As Integer, width As Integer, height As Integer)
            Me.X = x
            Me.Y = y
            Me.Width = width
            Me.Height = height

            SetBackground(Color.Black)

        End Sub

        Public Sub SetBackground(color As Color)
            m_color1 = color
            m_color2 = Nothing
            m_texturesDirectory = Nothing
            m_imageName = Nothing
            m_type = TextureTypes.Color
        End Sub

        Public Sub SetBackground(color1 As Color, color2 As Color)
            m_color1 = color1
            m_color2 = color2
            m_texturesDirectory = Nothing
            m_imageName = Nothing
            m_type = TextureTypes.GradientColor
        End Sub

        Public Sub SetBackground(texturesDirectory As DirectoryInfo, imageName As String)
            m_color1 = Nothing
            m_color2 = Nothing
            m_texturesDirectory = texturesDirectory
            m_imageName = imageName
            m_type = TextureTypes.Image
        End Sub

        Public Function GetBackground() As Brush

            Dim bkg As Brush = Nothing

            Select Case m_type
                Case TextureTypes.Color
                    bkg = New SolidBrush(m_color1)

                Case TextureTypes.GradientColor
                    bkg = New LinearGradientBrush(New Point(X, 1), New Point(Height, 1), m_color1, m_color2)

                Case TextureTypes.Image
                    Dim f As FileInfo = FileTool.FindImage(m_texturesDirectory, m_imageName & ".*")

                    If f IsNot Nothing Then
                        Dim img As Image = New Bitmap(f.FullName)
                        bkg = New TextureBrush(img, WrapMode.Clamp)
                    End If

            End Select

            If bkg Is Nothing Then
                bkg = New SolidBrush(Color.Black)
            End If

            Return bkg

        End Function

    End Class

End Namespace