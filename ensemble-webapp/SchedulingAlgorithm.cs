using System;

namespace ensemble_webapp {

  public class SchedulingAlgorithm {

    private int eventID;
   
    
    public SchedulingAlgorithm(int ID) {
      eventID = ID;
    }

    //connect to schedule home view model
    ScheduleHomeVM model = new SchedulesHomeVM();
    model.CurrentUser = Globals.LOGGED_IN_USER;
    
    //connect to DAL
    GetDAL dal = new GetDAL();
    dal.OpenConnection();
    
    
    //determine event
    Event event = null;
    event = GetEventByID(eventID);
    
    
    //make list of all rehearsal parts
    List<RehearsalPart> rehearsalParts = new List<RehearsalPart>;
    rehearsalParts = dal.GetRehearsalPartsByEvent(event);
    
    
    //make list of all rehearsals
    List<Rehearsal> rehearsals = new List<Rehearsal>;
    rehearsals = dal.GetRehearsalsByEvent(event);
        
    
    public sort {
      //loop goes through each rehearsal to find possible rehearsal parts
      for(int x = 0; rehearsals.item[x] != null; x++){
        
        //find and keep track of length of rehearsal being scheduled
        int rLength;
        rLength = rehearsals.item[x].dtmEndDateTime.subtract(rehearsals.item[x].dtmStartDateTime);
        
        //loop goes through each unscheduled rehearsal part 
        for(int i = 0; rLength > 0 && rehearsalParts.item[i] != null; i++){
          
          int numAvailable;
          //SQL stmt to find number of members available during part i
          string[] available = new string[numAvailable];//loop to load available members into array
            
          int numCalled;
          //SQL stmt to find number of members called to part
          string[] called = new string[numCalled];//loop to load called members into array
            
          int canAttend = 0; //counter for called members that are available
            
          //nested loops compare called members to members available during potential rehearsal part
          for(int j = 0; j < numCalled; j++){
            for(int k = 0; k < numAvailable; k++){
              if(called[j] == available[k]){
                canAttend++;
                k = numAvailable;
              }
            }
          }
            
          if(canAttend == numCalled){
            //SQL stmt to assign part i to rehearsal x
            //SQL stmt to add members to callboard
            rehearsalParts[i] = null;
          }
         
        }
      }
    }
  }
}
