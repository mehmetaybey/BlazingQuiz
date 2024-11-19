using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlazingQuiz.Api.Enums;

namespace BlazingQuiz.Api.Data.Entities
{
    public class StudentQuiz
    {
        [Key]
        public Guid Id { get; set; }

        public Guid QuizId { get; set; }

        public Guid StudentId { get; set; }

        public DateTime StartedOn { get; set; }

        public DateTime? CompletedOn { get; set; }

        [AllowedValues(
            nameof(StudentQuizStatus.Started),
            nameof(StudentQuizStatus.Completed),
            nameof(StudentQuizStatus.AutoSubmitted),
            nameof(StudentQuizStatus.Exited))]
        public string Status { get; set; } = nameof(StudentQuizStatus.Started);

        public int Score { get; set; }

        [ForeignKey(nameof(StudentId))]
        public virtual User Student { get; set; }

        [ForeignKey(nameof(QuizId))]
        public virtual Quiz Quiz { get; set; }

        public virtual ICollection<StudentQuizQuestion> StudentQuizQuestions { get; set; } = [];
    }
}
