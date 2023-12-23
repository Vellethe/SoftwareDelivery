## Description
I've done 4 tests to check different functionalities in my backend code from this Todo app, its testing the most commonly used methods in the app. 
I have also implmented a Dispose() method that is supposed to clear out the _context after every test is run to ensure that the resources and tests are properly cleaned up and removed again to not have any potential errors or issue when rerunning the tests

###AddNote() 
Tests the method should return an OkResult if the new note is added successfully.
###DeleteNote() 
Tests that the method successfully removes the note with that specific ID. In my test it removes the note with ID 2 and checks the deleted ID that the field is actually empty and then checks that the remaining notes are still there as intended. This test has been done by using different IDs and not just 2 and been working everytime.
###ClearCompletedNotes()
Tests that the method successfully removes all the completed notes, check so that the "completed section" is empty. The equal checks that all notes that are "false" are still there and that the first false note is existing. This test has been done with other notes being the first false note and its been working everytime.
###ReturnsNumberOfRemainingNotes()
Tests that the method returns all the incompleted notes, a similar test to the ClearCompletedNotes() but instead of deleting the completed ones it just reads through the incompleted and confirms how many of them there are.

##Delivery/Performance 
All the tests are done and green. Did some minor refactoring on the controllers and tried running the tests again after that, the tests came back green and successful once again.

##Delivered 2023-12-23