Imports CardMasterCard.Card
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO
Imports System.Reflection
Imports CardMasterSkin.GraphicsElement

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

        Private WORDS_SEPARATOR As Char = " ".Chars(0)

        Private m_text As String

        Public Property TextAttribute As String
        Public Property TextAlign As HorizontalAlignment = HorizontalAlignment.Left
        Public Property TextVerticalAlign As VerticalAlignment = VerticalAlignment.Top
        Public Property SymbolsDirectory As DirectoryInfo = Nothing

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

        Protected Overrides Function GetGraphicElements(card As Card) As List(Of GraphicElement)

            Dim textFontFamily As New FontFamily("Bell MT")
            Dim textFontStyle As FontStyle = FontStyle.Bold
            Dim textFontSize As Single = 13
            Dim textEmFontSize As Single = m_graphics.DpiY * textFontSize / 72
            Dim textFont As Font = New Font(textFontFamily, textFontSize, textFontStyle)
            Dim textFormat As StringFormat = StringFormat.GenericDefault

            Dim rowsList As New List(Of TextRow)
            Dim rowTop As Integer = 0

            ' Découpage en bloc de chaînes (découpage par "retour à la ligne")
            Dim textList As List(Of String) = GetTextList(card)

            'Dim rectList As List(Of Rectangle) = GetRectList(textList, textFont, textEmFontSize, textFormat)

            ' Dessine le contour de la zone max du texte
            'm_graphics.DrawRectangle(Pens.Red, Me.X, Me.Y, Me.Width, Me.Height)

            For Each txt As String In textList

                ' Recherche des symboles à charger.
                ' --> Retourne une liste composée de StringElement qui sont soit un mot soit un symbole.
                ' Si la chaine ne contient pas de mot à remplacer par un symbole, un string element contenant
                '  l'ensemble de la chaîne sera retourné.
                Dim textElementsList As List(Of TextElement) = GetTextElements(txt)

                ' Chargement des images (symboles)
                LoadSymbolsImages(textElementsList)

                ' Calcul de la taille et de la position en X,Y des TextElement (mots et symboles) composant le texte,
                ' et retourne une liste de lignes de texte composées de mots et de symboles.
                rowsList.AddRange(GetRows(textElementsList, textFont, textFormat, rowTop))

                ' Calcul de la position verticale de la prochaine ligne
                If rowsList.Count > 0 Then
                    rowTop = rowsList.Last.Bottom
                End If

            Next

            ApplyAlignments(rowsList)

            Return GetGraphicElementsList(rowsList, textFont, textEmFontSize, textFormat)


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

        Private Function GetTextElements(Text As String) As List(Of TextElement)

            Dim textElementsList As New List(Of TextElement)
            Dim textElement As TextElement

            Dim separators() As Char = {WORDS_SEPARATOR}

            Dim wordsArray() As String = Text.Split(separators, StringSplitOptions.RemoveEmptyEntries)

            For Each word As String In wordsArray

                textElement = New TextElement()
                textElement.Text = word
                textElementsList.Add(textElement)

            Next

            Return textElementsList

        End Function

        Private Sub LoadSymbolsImages(TextElementsList As List(Of TextElement))

            Dim searchPattern As String

            Dim symbolsDirExists As Boolean = (Me.SymbolsDirectory IsNot Nothing AndAlso Me.SymbolsDirectory.Exists)

            For Each element As TextElement In TextElementsList

                If element.Text Like "<<*>>" Then

                    element.Text = element.Text.Substring(2, element.Text.Length - 4)
                    searchPattern = element.Text & ".*"

                    If symbolsDirExists Then
                        With Me.SymbolsDirectory.GetFiles(searchPattern, SearchOption.AllDirectories)

                            If .Count > 0 Then
                                element.Image = Bitmap.FromFile(.First.FullName)
                            End If

                        End With
                    End If

                End If

            Next

        End Sub

        Private Function GetRows(TextElementsList As List(Of TextElement), textFont As Font, textFormat As StringFormat, rowTop As Integer) As List(Of TextRow)

            Dim rowsList As New List(Of TextRow)
            Dim row As TextRow = Nothing

            Dim elementSize As SizeF = Nothing

            Dim refSize As SizeF = Nothing
            Dim initSize As SizeF = Nothing
            Dim ratio As Single = 0

            Dim elementX As Integer = 0
            Dim rowY As Integer = rowTop

            Dim isSeparator As Boolean = False

            For Each textElement As TextElement In TextElementsList

                isSeparator = False

                If row Is Nothing Then
                    row = New TextRow
                    row.Y = rowY
                    rowsList.Add(row)
                End If

                If textElement.IsText Then
                    ' Ajout d'un mot à une ligne
                    elementSize = m_graphics.MeasureString(textElement.Text, textFont, Me.Width, textFormat)
                    isSeparator = (textElement.Text = WORDS_SEPARATOR)
                Else
                    ' Ajout d'un symbole à une ligne

                    If refSize = Nothing Then
                        refSize = m_graphics.MeasureString("O", textFont, Me.Width, textFormat)
                    End If

                    initSize = textElement.Image.GetBounds(m_graphics.PageUnit).Size

                    ratio = initSize.Height / refSize.Height
                    elementSize = New SizeF(initSize.Width / ratio, refSize.Height)

                End If

                If elementX + elementSize.Width > Me.Width Then

                    If isSeparator Then
                        ' Si le nouvel élément à ajouter dépasse la zone autorisée et est un séparateur de mots (caractère espace)
                        ' alors il est ignoré

                        ' Force la création d'une nouvelle ligne si il y a un nouvel élément à ajouter
                        row = Nothing

                        ' Ignore le textElement courant en forçant le Next
                        Continue For
                    End If

                    rowY = row.Bottom
                    row = New TextRow
                    row.Y = rowY
                    elementX = 0
                    rowsList.Add(row)

                End If

                textElement.Bounds = New Rectangle(elementX, 0, Math.Ceiling(elementSize.Width), Math.Ceiling(elementSize.Height))
                row.AddElement(textElement)
                elementX += textElement.Bounds.Width
            Next

            Return rowsList

        End Function

        Private Sub ApplyAlignments(rowsList As List(Of TextRow))

            If TextAlign = HorizontalAlignment.Right Then

                For Each row As TextRow In rowsList
                    row.X = Me.Width - row.Width
                Next

            ElseIf TextAlign = HorizontalAlignment.Center Then

                For Each row As TextRow In rowsList
                    row.X = (Me.Width - row.Width) \ 2
                Next

            End If

            If TextVerticalAlign <> VerticalAlignment.Top Then

                Dim rowsHeight As Integer = 0
                Dim y As Integer = 0

                For Each row As TextRow In rowsList
                    rowsHeight += row.Height
                Next

                If TextVerticalAlign = VerticalAlignment.Center Then
                    y = (Me.Height - rowsHeight) \ 2
                ElseIf TextVerticalAlign = VerticalAlignment.Bottom Then
                    y = Me.Height - rowsHeight
                End If

                For Each row As TextRow In rowsList
                    row.Y += y
                Next

            End If

        End Sub

        Private Function GetGraphicElementsList(rowsList As List(Of TextRow), textFont As Font, textEmFontSize As Single, textFormat As StringFormat) As List(Of GraphicElement)

            Dim graphicElementsList As New List(Of GraphicElement)
            Dim textPath As GraphicsPath = Nothing
            Dim imagePath As GraphicsPath = Nothing

            Dim r As Rectangle

            For Each row As TextRow In rowsList

                For Each textElement As TextElement In row.Elements

                    If textElement.IsText Then

                        If textPath Is Nothing Then
                            textPath = New GraphicsPath
                            graphicElementsList.Add(New PathElement(textPath, GetBackground))
                        End If

                        r = New Rectangle(Me.X + row.X + textElement.Bounds.X, Me.Y + row.Y + textElement.Bounds.Y, textElement.Bounds.Width, textElement.Bounds.Height)

                        textPath.AddString(textElement.Text, textFont.FontFamily, textFont.Style, textEmFontSize, r, textFormat)

                        ' Dessine le contour de la zone "au plus près" du texte
                        'm_graphics.DrawRectangle(Pens.Black, r)

                    Else
                        r = New Rectangle(Me.X + row.X + textElement.Bounds.X, Me.Y + row.Y + textElement.Bounds.Y, textElement.Bounds.Width, textElement.Bounds.Height)
                        graphicElementsList.Add(New ImageElement(textElement.Image, r))

                    End If

                Next
            Next

            Return graphicElementsList

        End Function

        Private Class TextElement

            Public Property Text As String = ""
            Public Property Image As Image = Nothing
            Public Property Bounds As Rectangle = Nothing

            Public Function IsText() As Boolean
                Return Image Is Nothing
            End Function
            Public Function IsImage() As Boolean
                Return Image IsNot Nothing
            End Function

        End Class

        Private Class TextRow

            Public Property Elements As New List(Of TextElement)

            Public Property X As Integer = 0
            Public Property Y As Integer = 0
            Public Property Width As Integer = 0
            Public Property Height As Integer = 0

            Public ReadOnly Property Bottom As Integer
                Get
                    Return Me.Y + Me.Height
                End Get
            End Property

            Public ReadOnly Property Right As Integer
                Get
                    Return Me.X + Me.Width
                End Get
            End Property

            Public Sub AddElement(element As TextElement)

                Me.Elements.Add(element)

                With element.Bounds

                    If Me.Height < .Height Then
                        Me.Height = .Height
                    End If

                    If Me.Width < .Right Then
                        Me.Width = .Right
                    End If

                End With

            End Sub

        End Class

    End Class

End Namespace