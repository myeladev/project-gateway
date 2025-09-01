using System.Collections.Generic;
using ProjectGateway.Code.Scripts;

namespace ProjectGateway
{
    public class Bed : Furniture, IInteractable
    {
        public new List<string> GetInteractOptions(InteractContext context)
        {
            // Get the base prop interactions
            var interactList = base.GetInteractOptions(context);
            // Add the "pick up" interaction for items
            interactList.Add("Sleep");
            // Return the modified list
            return interactList;
        }
        
        public new void Interact(string option, InteractContext context)
        {
            base.Interact(option, context);
            switch (option)
            {
                case "Sleep":
                    if (MyPlayer.instance.sleep <= 90f)
                    {
                        MyPlayer.instance.Sleep(this);
                    }
                    else
                    {
                        FeedbackMessageUIManager.instance.ShowMessage("You're not tired enough to sleep");
                    }
                    break;
            }
        }
    }
}
