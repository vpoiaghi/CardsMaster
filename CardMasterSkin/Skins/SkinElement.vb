Imports CardMasterCard.Card
Imports CardMasterSkin.GraphicsElement
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

        Public Property X As Integer = 0
        Public Property Y As Integer = 0
        Public Property Width As Integer = 0
        Public Property Height As Integer = 0
        Public Property Shadow As SkinShadow = Nothing

        Protected m_color1 As Color = Nothing
        Protected m_color2 As Color = Nothing
        Protected m_imageName As String = Nothing
        Private m_type As TextureTypes
        Protected m_skin As Skin

        Protected m_graphics As Graphics = Nothing

        Protected MustOverride Function GetGraphicElements(card As Card) As List(Of GraphicElement)

        Protected Sub New(skin As Skin, width As Integer, height As Integer)
            Me.New(skin, 0, 0, width, height)
        End Sub

        Protected Sub New(skin As Skin, x As Integer, y As Integer, width As Integer, height As Integer)
            Me.X = x
            Me.Y = y
            Me.Width = width
            Me.Height = height

            Me.m_skin = skin

            SetBackground(Color.Black)

        End Sub

        Public Sub Draw(g As Graphics, card As Card)

            m_graphics = g

            Dim graphicElements As List(Of GraphicElement) = GetGraphicElements(card)

            For Each graphicElement As GraphicElement In graphicElements
                graphicElement.DrawShadow(g, Shadow)
                graphicElement.Draw(g)
            Next

            m_graphics = Nothing

        End Sub

        Public Sub SetBackground(color As Color)
            m_color1 = color
            m_color2 = Nothing
            m_imageName = Nothing
            m_type = TextureTypes.Color
        End Sub

        Public Sub SetBackground(color1 As Color, color2 As Color)
            m_color1 = color1
            m_color2 = color2
            m_imageName = Nothing
            m_type = TextureTypes.GradientColor
        End Sub

        Public Sub SetBackground(imageName As String)
            m_color1 = Nothing
            m_color2 = Nothing
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

                    If (m_skin.TexturesDirectory IsNot Nothing) AndAlso (m_skin.TexturesDirectory.Exists) Then

                        Dim files() As FileInfo = m_skin.TexturesDirectory.GetFiles(m_imageName & ".*", SearchOption.AllDirectories)

                        If files.Count > 0 Then
                            Dim img As Image = New Bitmap(files.First.FullName)
                            bkg = New TextureBrush(img, WrapMode.Clamp)
                        End If

                    End If

            End Select

            If bkg Is Nothing Then
                bkg = New SolidBrush(Color.Black)
            End If

            Return bkg

        End Function

    End Class

End Namespace