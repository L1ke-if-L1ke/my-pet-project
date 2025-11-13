using Domain.User;

public sealed class UserStatusWithoutBook : UserStatus
{
    public UserStatusWithoutBook(int key, string name) : base(0, "Без книги") { }
    public override bool CanExecuteAction() => false;

}
