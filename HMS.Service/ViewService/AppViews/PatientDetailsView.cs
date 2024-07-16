using HMS.Data.DataAccess;
using HMS.Data.Models;
using HMS.Service.ViewService.Controls;

namespace HMS.Service.ViewService.AppViews;

public class PatientDetailsView : View
{
	readonly IViewService viewService;
	readonly IEnvironment environment;
	readonly IUnitOfWorkFactory unitOfWorkFactory;

	public PatientDetailsView(IViewService viewService, IEnvironment environment, IUnitOfWorkFactory unitOfWorkFactory)
	{
		this.viewService = viewService;
		this.environment = environment;
		this.unitOfWorkFactory = unitOfWorkFactory;
	}

	public override void BuildView(ViewBuilder viewBuilder)
	{
		viewBuilder
			.AddControl(new PageHeader("DOTNET Hospital Management System", "My Details"))
			.AddControl(new NewLine())
			.AddControl(new Label($"{environment.CurrentUser.USR_FullName}'s details"))
			.AddControl(new NewLine())
			.AddControl(new OutputBox(string.Empty, "patient-details-output"){ Enabled = false })
			.AddControl(new ObjectDataField<PatientModel>("patient-details-data",
				new DataFieldProperty<PatientModel>("Patient ID", x => x.PAT_User.USR_ID),
				new DataFieldProperty<PatientModel>("Full Name", x => x.PAT_User.USR_FullName),
				new DataFieldProperty<PatientModel>("Address", x => $"{x.PAT_User.USR_Address_State}, {x.PAT_User.USR_Address_Postcode}, {x.PAT_User.USR_Address_Line1} {x.PAT_User.USR_Address_Line2}"),
				new DataFieldProperty<PatientModel>("Email", x => x.PAT_User.USR_Email),
				new DataFieldProperty<PatientModel>("Phone", x => x.PAT_User.USR_PhoneNumber)
                ));
	}

	public override void OnBecomeActive()
	{
		var unitOfWork = unitOfWorkFactory.CreateUnitOfWork();
		var patientRepository = unitOfWork.GetRepository<PatientModel>();
		var patientDetailsField = Q<ObjectDataField<PatientModel>>("patient-details-data");

		var targetPatient = patientRepository.GetWhere(x => x.PAT_USR_ID == environment.CurrentUser.USR_PK, includedProperties: [nameof(PatientModel.PAT_User)], 1).FirstOrDefault();
		if (targetPatient != null)
		{
			patientDetailsField.Set(targetPatient);
		}
		else
		{
			var outputBox = Q<OutputBox>("patient-details-output");
			outputBox.Enabled = true;
			outputBox.SetState("Error occurred loading patient details", OutputBox.OutputState.Error);
		}

	}

	public override void OnEscapePressed()
	{
		viewService.LoadLastView();
	}
}