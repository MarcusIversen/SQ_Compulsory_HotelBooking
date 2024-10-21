namespace HotelBooking.Specs.Steps;

[Binding]
public sealed class CreateBookingStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;

    public CreateBookingStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given("the first number is (.*)")]
    public void GivenTheFirstNumberIs(int number)
    {
        _scenarioContext.Pending();
    }

    [Given("the second number is (.*)")]
    public void GivenTheSecondNumberIs(int number)
    {
        _scenarioContext.Pending();
    }

    [When("the two numbers are added")]
    public void WhenTheTwoNumbersAreAdded()
    {
        _scenarioContext.Pending();
    }

    [Then("the result should be (.*)")]
    public void ThenTheResultShouldBe(int result)
    {
        _scenarioContext.Pending();
    }
}