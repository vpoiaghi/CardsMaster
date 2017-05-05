Imports System.Drawing
Imports CardMasterSkin.Skins

Namespace GraphicsElement

    Public MustInherit Class GraphicElement

        Public MustOverride Sub DrawShadow(g As Graphics, shadow As SkinShadow)
        Public MustOverride Sub Draw(g As Graphics)

    End Class

End Namespace