import "./App.css";
import React, { useState, useEffect } from "react";
import { v4 as uuidv4 } from 'uuid';
import { BrowserRouter as  Router, Switch, Route} from "react-router-dom";
import Header from "./Header";
import AddContact from "./AddContact";
import ContactList from "./ContactList";

function App() {
  const LOCAL_STORAGE_KEY = "contacts";  

  // Initialize state with data from localStorage
  const [contacts, setContacts] = useState(() => {
    const retriveContacts = localStorage.getItem(LOCAL_STORAGE_KEY);
    return retriveContacts ? JSON.parse(retriveContacts) : [];
  });

    // Add a new contact to the list
    const addContactHandler = (contact) => {
      setContacts([...contacts, {id: uuidv4(), ...contact}]);
    };

    const removeContactHandler = (id) =>{
      const newContactList = contacts.filter((contact) =>{
        return contact.id !== id; 
      });
      setContacts(newContactList);
    };
    
  // Save contacts to localStorage whenever the state changes
  useEffect(() => {
    localStorage.setItem(LOCAL_STORAGE_KEY, JSON.stringify(contacts));
  }, [contacts]);


  return (
    <div className="ui container">
      <Header />
      <AddContact addContactHandler={addContactHandler} />
      <ContactList contacts={contacts} getContactId={removeContactHandler} />
    </div>
  );
}

export default App;



