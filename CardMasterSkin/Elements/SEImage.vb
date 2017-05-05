Imports System.Drawing
Imports System.IO
Imports CardMasterCard.Card
Imports CardMasterSkin.GraphicsElement


Namespace Skins

    Public Class SEImage
        Inherits SkinElement

        Public Property ImageName As String = Nothing
        Public Property RootDirectory As DirectoryInfo = Nothing

        Public Sub New(width As Integer, height As Integer, imageName As String)
            Me.New(0, 0, width, height, imageName)
        End Sub

        Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer, imageName As String)
            MyBase.New(x, y, width, height)

            Me.ImageName = imageName

        End Sub

        Protected Overrides Function GetGraphicElements(card As Card) As List(Of GraphicElement)

            Dim graphicElementsList As New List(Of GraphicElement)()

            If (RootDirectory IsNot Nothing) AndAlso (RootDirectory.Exists) Then

                Dim filePath As String = Nothing

                With RootDirectory.GetFiles(Me.ImageName & ".*", SearchOption.AllDirectories)
                    If .Count > 0 Then
                        filePath = .First.FullName
                    End If
                End With

                If filePath IsNot Nothing Then

                    Dim img As Image = Bitmap.FromFile(filePath, True)

                    graphicElementsList.Add(New ImageElement(img, New Rectangle(Me.X, Me.Y, Me.Width, Me.Height)))

                End If

            End If

            Return graphicElementsList

        End Function


    End Class

End Namespace