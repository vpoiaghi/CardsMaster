Imports System.Drawing
Imports System.IO
Imports System.Reflection
Imports CardMasterCard.Card
Imports CardMasterSkin.GraphicsElement


Namespace Skins

    Public Enum ResourceTypes
        Image
        Texture
    End Enum

    Public Class SEImage
        Inherits SkinElement

        Public Property ImageName As String = Nothing
        Public Property NameAttribute As String
        Public Property ResourceType As ResourceTypes = ResourceTypes.Texture

        Public Sub New(skin As Skin, width As Integer, height As Integer)
            Me.New(skin, 0, 0, width, height, Nothing)
        End Sub

        Public Sub New(skin As Skin, width As Integer, height As Integer, imageName As String)
            Me.New(skin, 0, 0, width, height, imageName)
        End Sub

        Public Sub New(skin As Skin, x As Integer, y As Integer, width As Integer, height As Integer)
            Me.New(skin, x, y, width, height, Nothing)
        End Sub

        Public Sub New(skin As Skin, x As Integer, y As Integer, width As Integer, height As Integer, imageName As String)
            MyBase.New(skin, x, y, width, height)

            Me.ImageName = imageName

        End Sub

        Protected Overrides Function GetGraphicElements(card As Card) As List(Of GraphicElement)

            Dim graphicElementsList As New List(Of GraphicElement)()

            Dim fileFullname = GetFileFullname(card)

            If fileFullname IsNot Nothing Then

                Dim img As Image = Bitmap.FromFile(fileFullname, True)

                graphicElementsList.Add(New ImageElement(img, New Rectangle(Me.X, Me.Y, Me.Width, Me.Height)))

            End If

            Return graphicElementsList

        End Function

        Private Function GetFileFullname(card As Card) As String

            Dim fileFullname As String = Nothing

            Dim rootDirectory As DirectoryInfo = Nothing

            Select Case Me.ResourceType
                Case ResourceTypes.Image : rootDirectory = m_skin.ImagesDirectory
                Case Else : rootDirectory = m_skin.TexturesDirectory
            End Select


            If (RootDirectory IsNot Nothing) AndAlso (RootDirectory.Exists) Then

                Dim fileName As String = Nothing

                If Not String.IsNullOrEmpty(Me.NameAttribute) Then

                    Dim cardType As Type = GetType(Card)
                    Dim cardProperty As PropertyInfo = cardType.GetProperty(Me.NameAttribute)
                    Dim cardGetMethod As MethodInfo = cardProperty.GetGetMethod
                    Dim propertyValue As Object = cardGetMethod.Invoke(card, Nothing)

                    If propertyValue IsNot Nothing Then
                        fileName = propertyValue.ToString
                    End If

                Else
                    fileName = ImageName
                End If

                If fileName IsNot Nothing Then

                    fileName = fileName.Replace(":", "-").Replace("""", "'")

                    With rootDirectory.GetFiles(fileName & ".*", SearchOption.AllDirectories)
                        If .Count > 0 Then
                            fileFullname = .First.FullName
                        End If
                    End With
                End If

            End If

            Return fileFullname

        End Function

    End Class

End Namespace