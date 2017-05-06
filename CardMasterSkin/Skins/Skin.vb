Imports System.Drawing
Imports System.IO

Namespace Skins

    Public Class Skin

        Public Property Width As Integer
        Public Property Height As Integer
        Public Property ImagesDirectory As DirectoryInfo
        Public Property TexturesDirectory As DirectoryInfo
        Public Property Elements As New SkinElements


        Protected Sub New()
        End Sub

        Public Sub New(width As Integer, height As Integer, imagesDirectory As DirectoryInfo, texturesDirectory As DirectoryInfo)

            Me.Width = width
            Me.Height = height
            Me.ImagesDirectory = imagesDirectory
            Me.TexturesDirectory = texturesDirectory

        End Sub

        Protected Overrides Sub Finalize()

            Me.ImagesDirectory = Nothing
            Me.TexturesDirectory = Nothing
            Me.Elements.Clear()

            MyBase.Finalize()
        End Sub
    End Class

End Namespace