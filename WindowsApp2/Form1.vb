Public Class Form1
    Private ReadOnly dao As New QuizDao()
    Private genres As List(Of Genre)
    Private btnRadios As List(Of RadioButton)
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        genres = dao.GetGenres()
        btnRadios = New List(Of RadioButton)

        For i = 0 To genres.Count - 1
            btnRadios.Add(New RadioButton() With {
                          .Name = "radio" & (i + 1),
                          .Text = genres(i).Name,
                          .Location = New Point(12, 12 + i * 25)
                          })
        Next
        btnRadios(0).Checked = True
        Panel1.Controls.AddRange(btnRadios.ToArray)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim checkindex = 0
        For i = 1 To btnRadios.Count
            If btnRadios(i - 1).Checked Then
                checkindex = i
            End If
        Next
        Dim form2 As New Form2 With {
            .MainForm = Me,
            .Quizzes = dao.GetQuizzes(checkindex)}
        Hide()
        form2.Show()
    End Sub
End Class
