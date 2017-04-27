Imports CardMasterCard.Card
Imports System.IO

Public Class Frm_Main

    Private m_file As FileInfo
    Private m_cardsProject As CardsProject = Nothing
    Private m_projectChanged As Boolean = False

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()


        GridData1.AddColumn("Nom").Width = 120
        GridData1.AddColumn("Genre", CellTypes.Combo).Width = 60
        GridData1.AddColumn("Rareté/Grade").Width = 85
        GridData1.AddColumn("Equipe").Width = 60
        GridData1.AddColumn("Nature chakra").Width = 90
        GridData1.AddColumn("Element").Width = 90
        GridData1.AddColumn("Coût").Width = 40
        GridData1.AddColumn("Atk").Width = 40
        GridData1.AddColumn("Def.").Width = 40
        GridData1.AddColumn("Pouvoir", CellTypes.RichText).Width = 400
        GridData1.AddColumn("Citation").Width = 120
        GridData1.AddColumn("Commentaires").Width = 120
        GridData1.AddColumn("Texture de fond").Width = 100
        GridData1.AddColumn("Texture zones de texte").Width = 100


        TSB_Save.Enabled = False
        TSB_SaveAs.Enabled = False
        TSB_Parameters.Enabled = False

    End Sub

    Private Sub TSB_Open_Click(sender As Object, e As EventArgs) Handles TSB_Open.Click
        Open()
    End Sub

    Private Sub TSB_Save_Click(sender As Object, e As EventArgs) Handles TSB_Save.Click
        Save()
    End Sub

    Private Sub TSB_SaveAs_Click(sender As Object, e As EventArgs) Handles TSB_SaveAs.Click
        SaveAs()
    End Sub

    Private Sub TSB_Parameters_Click(sender As Object, e As EventArgs) Handles TSB_Parameters.Click

        If m_cardsProject IsNot Nothing Then

            Dim prm As New Parameters
            prm.ImagesDirectory = m_cardsProject.ImagesDirectory
            prm.TexturesDirectory = m_cardsProject.TexturesDirectory

            Dim result As ParametersResult = Frm_Parameters.ChangeParameters(Me, prm)

            If result.DialogResult = Windows.Forms.DialogResult.OK Then

                m_cardsProject.TexturesDirectory = result.Parameters.TexturesDirectory
                m_cardsProject.ImagesDirectory = result.Parameters.ImagesDirectory

                DrawCard(CType(GridData1.SelectedRow().Tag, Card))

                If m_file IsNot Nothing Then
                    Save()
                End If

            End If

        End If

    End Sub

    Private Sub GridData1_SelectionChanged(sender As Object, e As GridDataSelectionChangedEventArgs) Handles GridData1.SelectionChanged

        If e.GetRow Is Nothing Then
            EraseCard()
        Else
            DrawCard(CType(e.GetRow.Tag, Card))
        End If

    End Sub

    Private Sub DrawCard(card As Card)

        If card Is Nothing Then
            EraseCard()
        Else
            Vwv_CardViewer.ShowCard(card, New DirectoryInfo(m_cardsProject.TexturesDirectory))
        End If

    End Sub

    Private Sub EraseCard()
        Vwv_CardViewer.EraseCard()
    End Sub

End Class
