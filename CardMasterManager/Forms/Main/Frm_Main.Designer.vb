<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Frm_Main
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Frm_Main))
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.TSB_Open = New System.Windows.Forms.ToolStripButton()
        Me.TSB_Save = New System.Windows.Forms.ToolStripButton()
        Me.TSB_SaveAs = New System.Windows.Forms.ToolStripButton()
        Me.Vwv_CardViewer = New CardMasterViewer.Viewer()
        Me.GridData1 = New CardMasterManager.GridData()
        Me.ToolStrip1.SuspendLayout()
        CType(Me.Vwv_CardViewer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 585)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(1268, 43)
        Me.Panel2.TabIndex = 4
        '
        'ToolStrip1
        '
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TSB_Open, Me.TSB_Save, Me.TSB_SaveAs})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.Size = New System.Drawing.Size(1268, 25)
        Me.ToolStrip1.TabIndex = 6
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'TSB_Open
        '
        Me.TSB_Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.TSB_Open.Image = CType(resources.GetObject("TSB_Open.Image"), System.Drawing.Image)
        Me.TSB_Open.ImageTransparentColor = System.Drawing.Color.Black
        Me.TSB_Open.Name = "TSB_Open"
        Me.TSB_Open.Size = New System.Drawing.Size(44, 22)
        Me.TSB_Open.Text = "Ouvrir"
        '
        'TSB_Save
        '
        Me.TSB_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.TSB_Save.Image = CType(resources.GetObject("TSB_Save.Image"), System.Drawing.Image)
        Me.TSB_Save.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSB_Save.Name = "TSB_Save"
        Me.TSB_Save.Size = New System.Drawing.Size(67, 22)
        Me.TSB_Save.Text = "Enregistrer"
        '
        'TSB_SaveAs
        '
        Me.TSB_SaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.TSB_SaveAs.Image = CType(resources.GetObject("TSB_SaveAs.Image"), System.Drawing.Image)
        Me.TSB_SaveAs.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TSB_SaveAs.Name = "TSB_SaveAs"
        Me.TSB_SaveAs.Size = New System.Drawing.Size(95, 22)
        Me.TSB_SaveAs.Text = "Enregistrer Sous"
        '
        'Vwv_CardViewer
        '
        Me.Vwv_CardViewer.Dock = System.Windows.Forms.DockStyle.Left
        Me.Vwv_CardViewer.Location = New System.Drawing.Point(0, 25)
        Me.Vwv_CardViewer.Name = "Vwv_CardViewer"
        Me.Vwv_CardViewer.Size = New System.Drawing.Size(383, 560)
        Me.Vwv_CardViewer.TabIndex = 7
        Me.Vwv_CardViewer.TabStop = False
        '
        'GridData1
        '
        Me.GridData1.AutoScroll = True
        Me.GridData1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.GridData1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridData1.Location = New System.Drawing.Point(383, 25)
        Me.GridData1.Name = "GridData1"
        Me.GridData1.Size = New System.Drawing.Size(885, 560)
        Me.GridData1.TabIndex = 5
        '
        'Frm_Main
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1268, 628)
        Me.Controls.Add(Me.GridData1)
        Me.Controls.Add(Me.Vwv_CardViewer)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Name = "Frm_Main"
        Me.Text = "CardMasterEdition 0.1"
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        CType(Me.Vwv_CardViewer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents GridData1 As GridData
    Friend WithEvents ToolStrip1 As System.Windows.Forms.ToolStrip
    Friend WithEvents TSB_Open As System.Windows.Forms.ToolStripButton
    Friend WithEvents TSB_Save As System.Windows.Forms.ToolStripButton
    Friend WithEvents TSB_SaveAs As System.Windows.Forms.ToolStripButton
    Friend WithEvents Vwv_CardViewer As CardMasterViewer.Viewer

End Class
