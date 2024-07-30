using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class AddUserView<T> : View where T : class, IUser, IDbModel
{
	readonly IViewService viewService;
	readonly IUnitOfWorkFactory unitOfWorkFactory;

	string? firstName;
	string? lastName;
	string? email;
	int phone;
	int postCode;
	string? state;
	string? addressLine1;
	string? addressLine2;
	string? password;

	OutputBox outputBox;

	public AddUserView(IViewService viewService, IUnitOfWorkFactory unitOfWorkFactory)
	{
		this.viewService = viewService;
		this.unitOfWorkFactory = unitOfWorkFactory;
	}

	/// <summary>
	/// We override builder, to provide implementation of build view. Using view builder we can add properties
	/// </summary>
	/// <param name="viewBuilder"></param>
	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "Add User"))
			.AddControl(new NewLine())
			.AddControl(new OutputBox("OutputBox"))
			.Place(ref outputBox)
			.AddControl(new InputField("First Name: ") { MaxLength = 20 }).Setup<InputField>(x => x.BindProperty<string>(prop => firstName = prop, nameof(InputField.Contents)))
			.AddControl(new InputField("Last Name: ") { MaxLength = 20 }).Setup<InputField>(x => x.BindProperty<string>(prop => lastName = prop, nameof(InputField.Contents)))
			.AddControl(new InputField("Email Name: ") { MaxLength = 20 }).Setup<InputField>(x => x.BindProperty<string>(prop => email = prop, nameof(InputField.Contents)))
			.AddControl(new InputField("Phone: ") { MaxLength = 10 }).Setup<InputField>(x => x.BindProperty<string>(prop => int.TryParse(prop, out phone), nameof(InputField.Contents)))
			.AddControl(new InputField("PostCode: ") { MaxLength = 4 }).Setup<InputField>(x => x.BindProperty<string>(prop => int.TryParse(prop, out postCode), nameof(InputField.Contents)))
			.AddControl(new InputField("State: ") { MaxLength = 3 }).Setup<InputField>(x => x.BindProperty<string>(prop => state = prop, nameof(InputField.Contents)))
			.AddControl(new InputField("Address Line 1: ") { MaxLength = 20 }).Setup<InputField>(x => x.BindProperty<string>(prop => addressLine1 = prop, nameof(InputField.Contents)))
			.AddControl(new InputField("Address Line 2: ") { MaxLength = 20 }).Setup<InputField>(x => x.BindProperty<string>(prop => addressLine2 = prop, nameof(InputField.Contents)))
			.AddControl(new InputField("Password: ") { ObscureContent = true }).Setup<InputField>(x => x.BindProperty<string>(prop => password = prop, nameof(InputField.Contents)))
			.AddControl(new NewLine())
            .AddControl(new Button("Add User", "add-user"))
			.Setup<Button>(x => x.Interacted += AddUser);
	}

	//Users bound properties from the input field to create a new user
	void AddUser(object? sender, EventArgs e)
	{
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var repository = unitOfWork.GetRepository<T>();

		if (
			string.IsNullOrEmpty(firstName) || 
			string.IsNullOrEmpty(lastName) ||
			string.IsNullOrEmpty(email) ||
			string.IsNullOrEmpty(addressLine1) ||
			string.IsNullOrEmpty(addressLine2) ||
			string.IsNullOrEmpty(password) ||
			string.IsNullOrEmpty(state)
		)
		{
			outputBox.Enabled = true;
			outputBox.SetState("Please make sure all fields are filled", OutputBox.OutputState.Warn);
			return;
		}

		if (!ValidateEmail(email))
		{
			outputBox.Enabled = true;
			outputBox.SetState("Make sure email is in the correct format", OutputBox.OutputState.Warn);
            return;
		}

		var random = new Random();
		var i = random.Next();
        var user = new UserModel()
		{
			USR_ID = i,
			USR_FirstName = firstName,
			USR_LastName = lastName,
			USR_Password = password,
			USR_Email = email,
			USR_PhoneNumber = phone.ToString(),
			USR_Address_Line1 = addressLine1,
			USR_Address_Line2 = addressLine2,
			USR_Address_Postcode = postCode.ToString(),
			USR_Address_State = state
		};

		var model = Activator.CreateInstance(typeof(T)) as T 
			?? throw new InvalidOperationException($"Cannot create IUser for {typeof(T).Name}");
		model.User = user;
		repository.Insert(model);
		unitOfWork.Commit();

		outputBox.Enabled = true;
		outputBox.SetState($"Added new user {firstName} + {lastName}", OutputBox.OutputState.Success);
	}

	bool ValidateEmail(string email)
	{
		return email.Contains('@') && email.EndsWith(".com");
	}

	public override void OnEscapePressed()
	{
		viewService.LoadLastView();
	}
}