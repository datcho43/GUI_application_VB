Imports System
Imports Npgsql

Public Class Form1


    Public num As Integer
    Public question_count As Integer
    Public correct_count As Integer
    Public fast As Integer = 0
    Public quizzes As New List(Of Quiz)
    Public choices As New List(Of Choice)
    Public connectionString As String = "Server=localhost;Port=5432;User Id=postgres;Password=0403;DataBase=choise_quiz"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Using con As New NpgsqlConnection(connectionString)
            con.Open()
            Dim command As New NpgsqlCommand("SELECT * FROM quiz WHERE genre_id = @genre_id", con)
            command.Parameters.AddWithValue("genre_id", 1)
            command.Prepare()
            Dim reader As NpgsqlDataReader = command.ExecuteReader()
            While reader.Read()
                quizzes.Add(New Quiz(reader("id"), reader("genre_id"), reader("question")))
            End While
            reader.Close()
            command.Dispose()
            Util.Shuffle(quizzes)
            Dim quiz_ids As String = String.Join(",", quizzes.Select(Function(q) q.Id))
            '関数型リンク（ラムダ式）
            command = New NpgsqlCommand($"SELECT * FROM choice WHERE quiz_id IN ({quiz_ids});", con)
            reader = command.ExecuteReader()
            While reader.Read()
                choices.Add(New Choice(reader("id"), reader("quiz_id"), reader("choice"), reader("is_correct")))

            End While
            reader.Close()
            command.Dispose()
            For Each quiz As Quiz In quizzes
                quiz.Choices = New List(Of Choice)(choices.Where(Function(c) c.Quiz_id = quiz.Id))
                '問題と選択肢の連結
            Next
        End Using
    End Sub



    Public Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If question_count = 0 Then
            quizzes(0).ShowQuestion()
            question_count += 1
            Button1.Text = "SELECT"
        ElseIf question_count = 8 Then
            lblText01.Text = "おわり"

        Else

            If RadioButton1.Checked Then
                num = 1

            ElseIf RadioButton2.Checked Then
                num = 2

            ElseIf RadioButton3.Checked Then
                num = 3

            End If

            With quizzes(question_count)
                .ShowQuestion()
                If .IsCorrect(num) Then
                    correct_count += 1
                    lblText02.Text = $"正解　 : {question_count}問中 {correct_count}問正解"
                Else
                    lblText02.Text = $"不正解　 : {question_count}問中 {correct_count}問正解"
                End If
            End With
            question_count += 1
            If Not question_count = 9 Then
                quizzes(question_count).ShowQuestion()
            End If
        End If

    End Sub
End Class
Public Class Quiz
    Public ReadOnly Property Id() As String
    Public ReadOnly Property Genre_id() As String
    Public ReadOnly Property Question() As String
    Private _Choise As List(Of Choice)
    Public Property Choices() As List(Of Choice)
        Get
            Return _Choise
        End Get
        Set(value As List(Of Choice))
            _Choise = value
        End Set
    End Property
    Public Sub New(id As Integer, genre_id As Integer, question As String, choices As List(Of Choice))
        Me.Id = id
        Me.Genre_id = genre_id
        Me.Question = question
        Me.Choices = choices

    End Sub
    Public Sub New(id As Integer, genre_id As Integer, question As String)
        Me.Id = id
        Me.Genre_id = genre_id
        Me.Question = question
    End Sub
    Public Sub ShowQuestion()
        Form1.lblText01.Text = $"{Question}"
        Form1.RadioButton1.Text = $"{Choices(1 - 1).Choise}"
        Form1.RadioButton2.Text = $"{Choices(2 - 1).Choise}"
        Form1.RadioButton3.Text = $"{Choices(3 - 1).Choise}"

    End Sub

    Public Function IsCorrect(index As Integer) As Boolean
        Return Choices(index - 1).Is_correct
    End Function
End Class

Public Class Choice
    Public ReadOnly Property Id() As Integer
    Public ReadOnly Property Quiz_id() As Integer
    Public ReadOnly Property Choise() As String
    Public ReadOnly Property Is_correct As Boolean
    Public Sub New(id As Integer, quiz_id As Integer, choise As String, is_correct As Boolean)
        Me.Id = id
        Me.Quiz_id = quiz_id
        Me.Choise = choise
        Me.Is_correct = is_correct

    End Sub
End Class

Module Util
    Public Sub Shuffle(Of T)(list As IList(Of T))
        Dim r As Random = New Random()
        For i = 0 To list.Count - 1
            Dim index As Integer = r.Next(i, list.Count)
            If i <> index Then
                Dim temp As T = list(i)
                list(i) = list(index)
                list(index) = temp
            End If
        Next
    End Sub
End Module
