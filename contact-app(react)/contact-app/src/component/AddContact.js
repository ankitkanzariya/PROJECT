import React from "react";

class AddContact extends React.Component {
  state = {
    name: "",
    email: "",
  };
  add=(e)=> {
    e.preventDefault();
    if (this.state.name === "" || this.state.email === ""){
      alert("all fields are mandotary");
      return
    }
    this.props.addContactHandler(this.state);
    this.setState({name:"", email:""});
  }

  render() {
    return (
      <div className="ui main">
        <h2> Add contact </h2>
        <form className="ui form" onSubmit={this.add}>
          <div className="field">
            <label>Name</label>
            <input
              type="text"
              name="text"
              placeholder="name"
              value={this.state.name}
              onChange={(e) => this.setState({ name: e.target.value })}
            />
          </div>
          <div className="field">
            <label>Email</label>
            <input type="email"
             name="email"
              placeholder="Email"
              value={this.state.email}
              onChange={(e) => this.setState({ email: e.target.value })}
               />
            <button className="ui button blue">Add</button>
          </div>
        </form>
      </div>
    );
  }
}

export default AddContact;
