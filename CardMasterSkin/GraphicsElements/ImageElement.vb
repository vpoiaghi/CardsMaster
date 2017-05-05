Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports CardMasterSkin.Skins

Namespace GraphicsElement

    Public Class ImageElement
        Inherits GraphicElement

        Private m_image As Image
        Private m_rectangle As Rectangle

        Public Sub New(image As Image, rectangle As Rectangle)
            MyBase.New

            m_image = image
            m_rectangle = rectangle

        End Sub

        Public Overrides Sub Draw(g As Graphics)

            g.DrawImage(m_image, m_rectangle)

        End Sub

        Public Overrides Sub DrawShadow(g As Graphics, shadow As SkinShadow)

        End Sub
    End Class

End Namespace