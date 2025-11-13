using Domain.User;

public sealed class UserStatusWithBook : UserStatus
{
    public UserStatusWithBook(int key, string name) : base(1, "С книгой") { }
    public override bool CanExecuteAction() => true;
}