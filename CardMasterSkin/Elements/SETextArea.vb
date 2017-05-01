Imports CardMasterCard.Card
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Reflection

Namespace Skins

    Public Enum HorizontalAlignment
        Left
        Center
        Right
    End Enum

    Public Class SETextArea
        Inherits SkinElement

        Private m_text As String

        Public Property TextAttribute As String
        Public Property TextAlign As HorizontalAlignment = HorizontalAlignment.Left

        Public Sub New(width As Integer, height As Integer)
            Me.New(0, 0, width, height, Nothing)
        End Sub

        Public Sub New(width As Integer, height As Integer, text As String)
            Me.New(0, 0, width, height, text)
        End Sub

        Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer)
            Me.New(x, y, width, height, Nothing)
        End Sub

        Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer, text As String)
            MyBase.New(x, y, width, height)

            m_text = text
            SetBackground(Color.Black)

        End Sub

        Public Overrides Function GetPath(card As Card) As GraphicsPath

            Dim path As New GraphicsPath()

            Dim stringText As String = GetText(card)
            Dim family As New FontFamily("Bell MT")
            'Dim myfontStyle As Integer = CInt(FontStyle.Italic)
            Dim myfontStyle As Integer = CInt(FontStyle.Regular)
            Dim emSize As Integer = 18
            Dim format As StringFormat = StringFormat.GenericDefault

            Dim textFont As Font = New Font(family, emSize, FontStyle.Regular)

            Dim g As Graphics = Graphics.FromImage(New Bitmap(10, 10))
            Dim textSize As SizeF = g.MeasureString(stringText, textFont, New SizeF(Width, Height), format)
            Dim textX As Integer = X
            Dim textY As Integer = Y

            If Me.TextAlign = HorizontalAlignment.Center Then
                If textSize.Width < Me.Width Then
                    textX += (Me.Width - textSize.Width) \ 2
                End If
            ElseIf Me.TextAlign = HorizontalAlignment.Right Then
                textX += X + Width - textSize.Width
            End If

            If textSize.Height < Me.Height Then
                textY += (Me.Height - textSize.Height) \ 2
            End If

            'Dim origin As New Point(textX, textY)
            Dim origin As New Rectangle(textX, textY, textSize.Width, textSize.Height)

            path.AddString(stringText, family, myfontStyle, emSize, origin, format)

            Return path

        End Function

        Private Function GetText(card As Card) As String

            Dim txt = ""

            If Not String.IsNullOrEmpty(Me.TextAttribute) Then

                Dim cardType As Type = GetType(Card)
                Dim cardProperty As PropertyInfo = cardType.GetProperty(Me.TextAttribute)
                Dim cardGetMethod As MethodInfo = cardProperty.GetGetMethod
                Dim propertyValue As Object = cardGetMethod.Invoke(card, Nothing)

                If propertyValue IsNot Nothing Then
                    txt = propertyValue.ToString
                End If

            Else
                txt = m_text
            End If

            Return txt

        End Function


    End Class

End Namespace