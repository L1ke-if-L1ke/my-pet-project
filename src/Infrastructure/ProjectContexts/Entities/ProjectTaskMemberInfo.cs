namespace Infrastructure.ProjectContexts.Entities
{
    public sealed record ProjectTaskMemberInfo
    {
        //Навигация к задаче
        public ProjectTask Task { get; private set; } = null!;

        //ID участника
        public ProjectMemberId MemberId { get; private set; }

        //Email участника
        public string MemberEmail { get; private set; } = string.Empty;

        //Логин участника
        public string MemberLogin { get; private set; } = string.Empty;

        public static ProjectTaskMemberInfo Create(
            ProjectMemberId memberId,
            string email,
            string login
        )
        {
            if (memberId == null)
                throw new ArgumentNullException(nameof(memberId));

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email участника не может быть пустым.", nameof(email));

            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException("Логин участника не может быть пустым.", nameof(login));

            if (email.Length > 255)
                throw new ArgumentException("Email слишком длинный.", nameof(email));

            if (login.Length > 100)
                throw new ArgumentException("Логин слишком длинный.", nameof(login));

            return new ProjectTaskMemberInfo
            {
                MemberId = memberId,
                MemberEmail = email,
                MemberLogin = login
            };
        }
    }
}
