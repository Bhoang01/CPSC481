public class StateContainer
{
	private List<User> users = new List<User>();
	private User? currentUser = null;
	public bool isLoggedIn() => currentUser != null;
	public string? getFirstName() => currentUser?.firstName;
	public string? signup(string firstName, string lastName, string email, string password)
	{
		if (currentUser != null)
			return "You are already signed in";
		if (users.Any(u => u.email == email))
			return "Email already exists";
		User user = new User(firstName, lastName, email, password);
		users.Add(user);
		currentUser = user;
		NotifyStateChanged();
		return null;
	}
	public string? login(string email, string password)
	{
		if (currentUser != null)
			return "You are already signed in";
		User? user = users.FirstOrDefault(u => u.email == email && u.password == password);
		if (user == null)
			return "Invalid email or password";
		currentUser = user;
		NotifyStateChanged();
		return null;
	}
	public bool logout()
	{
		if (currentUser == null)
			return false;
		currentUser = null;
		NotifyStateChanged();
		return true;
	}
	public event Action? OnChange;
	private void NotifyStateChanged() => OnChange?.Invoke();
}