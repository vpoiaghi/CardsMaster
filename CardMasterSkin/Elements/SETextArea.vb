Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Reflection
Imports CardMasterCard.Card

Namespace Skins

    Public Enum HorizontalAlignment
        Left
        Center
        Right
    End Enum

    Public Enum VerticalAlignment
        Top
        Center
        Bottom
    End Enum

    Public Class SETextArea
        Inherits SkinElement

        Private m_text As String

        Public Property TextAttribute As String
        Public Property TextAlign As HorizontalAlignment = HorizontalAlignment.Left
        Public Property TextVerticalAlign As VerticalAlignment = VerticalAlignment.top

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

        Public Overrides Function GetPathes(card As Card) As List(Of GraphicsPath)

            Dim pathes As New List(Of GraphicsPath)()
            Dim path As New GraphicsPath()

            Dim textFontFamily As New FontFamily("Bell MT")
            Dim textFontStyle As FontStyle = FontStyle.Regular
            Dim textFontSize As Single = 13
            Dim textEmFontSize As Single = m_graphics.DpiY * textFontSize / 72
            Dim textFont As Font = New Font(textFontFamily, textFontSize, textFontStyle)
            Dim textFormat As StringFormat = StringFormat.GenericDefault

            Dim textList As List(Of String) = GetTextList(card)
            Dim rectList As List(Of Rectangle) = GetRectList(textList, textFont, textEmFontSize, textFormat)

            ' Dessine le contour de la zone max du texte
            'm_graphics.DrawRectangle(Pens.Red, Me.X, Me.Y, Me.Width, Me.Height)

            For i As Integer = 0 To textList.Count - 1

                path.AddString(textList(i), textFontFamily, textFontStyle, textEmFontSize, rectList(i), textFormat)

                ' Dessine le contour de la zone "au plus près" du texte
                'm_graphics.DrawRectangle(Pens.Black, rectList(i))

            Next

            pathes.Add(path)

            Return pathes


        End Function

        Private Function GetTextList(card As Card) As List(Of String)

            Dim txtList As New List(Of String)
            Dim fullText As String = Nothing

            If Not String.IsNullOrEmpty(Me.TextAttribute) Then

                Dim cardType As Type = GetType(Card)
                Dim cardProperty As PropertyInfo = cardType.GetProperty(Me.TextAttribute)
                Dim cardGetMethod As MethodInfo = cardProperty.GetGetMethod
                Dim propertyValue As Object = cardGetMethod.Invoke(card, Nothing)

                If propertyValue IsNot Nothing Then
                    fullText = propertyValue.ToString
                End If

            Else
                fullText = m_text
            End If


            If Not String.IsNullOrEmpty(fullText) Then

                Dim txtArray() As String = fullText.ToString.Split(Environment.NewLine.ToCharArray, StringSplitOptions.RemoveEmptyEntries)

                For Each txt As String In txtArray
                    txtList.Add(txt)
                Next

            End If


            Return txtList

        End Function

        Private Function GetRectList(textList As List(Of String), textFont As Font, textEmFontSize As Single, textFormat As StringFormat) As List(Of Rectangle)

            Dim rectList As New List(Of Rectangle)

            Dim textRectangle As Rectangle
            Dim textX As Integer = Me.X
            Dim textY As Integer = Me.Y
            Dim textSize As SizeF
            Dim maxWidth As Integer = Me.Width
            Dim maxHeight As Integer = Me.Height

            Dim totalHeight As Integer = 0
            Dim translateY As Integer = 0

            For Each txt As String In textList

                Dim f As New Font(textFont.FontFamily, textEmFontSize, textFont.Style)

                textSize = m_graphics.MeasureString(txt, textFont, maxWidth, textFormat)

                Select Case Me.TextAlign
                    Case HorizontalAlignment.Left
                        textX = Me.X
                    Case HorizontalAlignment.Center
                        textX = Me.X + (maxWidth - textSize.Width) \ 2
                    Case HorizontalAlignment.Right
                        textX = Me.X + maxWidth - textSize.Width
                End Select

                If textX < 0 Then textX = 0
                If textY < 0 Then textY = 0

                textRectangle = New Rectangle(textX, textY, Me.Width, textSize.Height)
                rectList.Add(textRectangle)

                textY += textSize.Height
                maxHeight = maxHeight - textSize.Height
                totalHeight += textSize.Height

            Next

            Select Case Me.TextVerticalAlign
                Case VerticalAlignment.Top
                    translateY = 0
                Case VerticalAlignment.Center
                    translateY = (Me.Height - totalHeight) \ 2
                Case VerticalAlignment.Bottom
                    translateY = Me.Height - totalHeight
            End Select

            If translateY <> 0 Then

                Dim r As Rectangle

                For i As Integer = 0 To rectList.Count - 1
                    r = rectList(i)
                    r.Offset(0, translateY)
                    rectList(i) = r
                Next
            End If

            Return rectList

        End Function

    End Class

End Namespace