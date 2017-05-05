Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports CardMasterSkin.Skins

Namespace GraphicsElement

    Public Class PathElement
        Inherits GraphicElement

        Private m_path As GraphicsPath
        Private m_background As Brush

        Public Sub New(path As GraphicsPath, background As Brush)
            MyBase.New

            m_path = path
            m_background = background

        End Sub

        Public Overrides Sub Draw(g As Graphics)

            g.FillPath(m_background, m_path)

        End Sub

        Public Overrides Sub DrawShadow(g As Graphics, shadow As SkinShadow)

            If shadow IsNot Nothing Then

                Dim radianAngle As Double = shadow.Angle * Math.PI / 180

                Dim transactionX As Integer = Math.Cos(radianAngle) * shadow.Size
                Dim transactionY As Integer = Math.Sin(radianAngle) * shadow.Size

                Dim translateMatrix As New Matrix
                translateMatrix.Translate(transactionX, transactionY)

                Dim shadowPath As GraphicsPath = m_path.Clone()
                shadowPath.Transform(translateMatrix)

                Dim b As Brush = New SolidBrush(Color.FromArgb(120, 0, 0, 0))

                g.FillPath(b, shadowPath)

            End If

        End Sub

    End Class

End Namespace