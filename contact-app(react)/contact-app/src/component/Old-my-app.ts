// import "./App.css";
// import React, {useState, useEffect} from "react";
// import Header from "./Header";
// import AddContact from "./AddContact";
// import ContactList from "./ContactList";

function App() {
  const LOCAL_STORAGE_KEY = "contacts";  
  const [contacts, setContacts] = useState([]);

  useEffect(()=> {
    const retriveContacts = JSON.parse(localStorage.getItem(LOCAL_STORAGE_KEY));
    if (retriveContacts) setContacts(retriveContacts);
  },[]);

  const addContactHandler = (contact) => {
    console.log(contact);
    setContacts([...contacts,contact]);
  };

  useEffect(()=> {
    localStorage.setItem(LOCAL_STORAGE_KEY, JSON.stringify(contacts));
  },[contacts]);

  return (
    <div className="ui container">
      <Header />
      <AddContact addContactHandler = {addContactHandler} />
      <ContactList contacts = {contacts} />
    </div>
  );
}

export default App;
