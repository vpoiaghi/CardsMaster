<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_Parameters
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.Lbl_TexturesDirectory = New System.Windows.Forms.Label()
        Me.Lbl_ImagesDirectory = New System.Windows.Forms.Label()
        Me.Txt_TexturesDirectory = New System.Windows.Forms.TextBox()
        Me.Txt_ImagesDirectory = New System.Windows.Forms.TextBox()
        Me.Btn_TexturesDirectory = New System.Windows.Forms.Button()
        Me.Btn_ImagesDirectory = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(277, 274)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(146, 29)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Annuler"
        '
        'Lbl_TexturesDirectory
        '
        Me.Lbl_TexturesDirectory.AutoSize = True
        Me.Lbl_TexturesDirectory.Location = New System.Drawing.Point(12, 19)
        Me.Lbl_TexturesDirectory.Name = "Lbl_TexturesDirectory"
        Me.Lbl_TexturesDirectory.Size = New System.Drawing.Size(108, 13)
        Me.Lbl_TexturesDirectory.TabIndex = 1
        Me.Lbl_TexturesDirectory.Text = "Dossier des textures :"
        '
        'Lbl_ImagesDirectory
        '
        Me.Lbl_ImagesDirectory.AutoSize = True
        Me.Lbl_ImagesDirectory.Location = New System.Drawing.Point(12, 42)
        Me.Lbl_ImagesDirectory.Name = "Lbl_ImagesDirectory"
        Me.Lbl_ImagesDirectory.Size = New System.Drawing.Size(104, 13)
        Me.Lbl_ImagesDirectory.TabIndex = 2
        Me.Lbl_ImagesDirectory.Text = "Dossier des images :"
        '
        'Txt_TexturesDirectory
        '
        Me.Txt_TexturesDirectory.Location = New System.Drawing.Point(126, 16)
        Me.Txt_TexturesDirectory.Name = "Txt_TexturesDirectory"
        Me.Txt_TexturesDirectory.Size = New System.Drawing.Size(274, 20)
        Me.Txt_TexturesDirectory.TabIndex = 3
        '
        'Txt_ImagesDirectory
        '
        Me.Txt_ImagesDirectory.Location = New System.Drawing.Point(126, 39)
        Me.Txt_ImagesDirectory.Name = "Txt_ImagesDirectory"
        Me.Txt_ImagesDirectory.Size = New System.Drawing.Size(274, 20)
        Me.Txt_ImagesDirectory.TabIndex = 4
        '
        'Btn_TexturesDirectory
        '
        Me.Btn_TexturesDirectory.Location = New System.Drawing.Point(397, 16)
        Me.Btn_TexturesDirectory.Name = "Btn_TexturesDirectory"
        Me.Btn_TexturesDirectory.Size = New System.Drawing.Size(23, 20)
        Me.Btn_TexturesDirectory.TabIndex = 5
        Me.Btn_TexturesDirectory.Text = "..."
        Me.Btn_TexturesDirectory.UseVisualStyleBackColor = True
        '
        'Btn_ImagesDirectory
        '
        Me.Btn_ImagesDirectory.Location = New System.Drawing.Point(397, 39)
        Me.Btn_ImagesDirectory.Name = "Btn_ImagesDirectory"
        Me.Btn_ImagesDirectory.Size = New System.Drawing.Size(23, 20)
        Me.Btn_ImagesDirectory.TabIndex = 6
        Me.Btn_ImagesDirectory.Text = "..."
        Me.Btn_ImagesDirectory.UseVisualStyleBackColor = True
        '
        'Frm_Parameters
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(435, 315)
        Me.Controls.Add(Me.Btn_ImagesDirectory)
        Me.Controls.Add(Me.Btn_TexturesDirectory)
        Me.Controls.Add(Me.Txt_ImagesDirectory)
        Me.Controls.Add(Me.Txt_TexturesDirectory)
        Me.Controls.Add(Me.Lbl_ImagesDirectory)
        Me.Controls.Add(Me.Lbl_TexturesDirectory)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Frm_Parameters"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Paramètres du projet"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Lbl_TexturesDirectory As System.Windows.Forms.Label
    Friend WithEvents Lbl_ImagesDirectory As System.Windows.Forms.Label
    Friend WithEvents Txt_TexturesDirectory As System.Windows.Forms.TextBox
    Friend WithEvents Txt_ImagesDirectory As System.Windows.Forms.TextBox
    Friend WithEvents Btn_TexturesDirectory As System.Windows.Forms.Button
    Friend WithEvents Btn_ImagesDirectory As System.Windows.Forms.Button

End Class
