Imports CardMasterCard.Card
Imports System.IO
Imports System.Windows.Forms

Partial Public Class Frm_Main

    Private Sub Open()

        Dim op As New FileOpen
        Dim cardsProject As CardsProject = Nothing
        Dim file As FileInfo = Nothing

        file = op.GetFile(GetCurrentDir(m_file))

        If file IsNot Nothing Then
            cardsProject = op.LoadProject(file)

            If cardsProject IsNot Nothing Then

                With New ProjectToGridData()
                    .LoadProject(cardsProject, GridData1)
                End With

                m_file = file
                m_cardsProject = cardsProject
                m_projectChanged = False

                TSB_Save.Enabled = True
                TSB_SaveAs.Enabled = True

            End If

        End If

    End Sub

    Private Sub Save()
        Save(m_file)
    End Sub

    Private Sub SaveAs()

        Dim f As FileInfo = (New FileSave).GetFile(GetCurrentDir(m_file))
        Save(f)

    End Sub

    Private Sub Save(targetFile As FileInfo)

        If targetFile IsNot Nothing Then

            Dim prjUpdater As New GridDataToProject
            Dim cardsProject As New CardsProject

            prjUpdater.SaveProject(GridData1, cardsProject)

            With New FileSave
                .Save(cardsProject, targetFile)
            End With

            m_file = targetFile
            m_cardsProject = cardsProject
            m_projectChanged = False

        End If

    End Sub

    Private Function GetCurrentDir(file As FileInfo) As DirectoryInfo

        Dim d As DirectoryInfo

        If file Is Nothing Then
            d = New DirectoryInfo(Application.StartupPath)
        Else
            d = file.Directory
        End If

        Return d

    End Function

End Class
