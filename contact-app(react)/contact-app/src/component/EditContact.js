import React from "react";

class EditContact extends React.Component {
  state = {
    id: "",
    name: "",
    email: "",
  };

  componentDidMount() {
    const { id, name, email } = this.props.location.state.contact;
    this.setState({ id, name, email });
  }

  update = (e) => {
    e.preventDefault();
    if (this.state.name === "" || this.state.email === "") {
      alert("All fields are mandatory");
      return;
    }
    this.props.updateContactHandler(this.state);
    this.props.history.push("/");
  };

  render() {
    return (
      <div className="ui main">
        <h2>Edit Contact</h2>
        <form className="ui form" onSubmit={this.update}>
          <div className="field">
            <label>Name</label>
            <input
              type="text"
              placeholder="Name"
              value={this.state.name}
              onChange={(e) => this.setState({ name: e.target.value })}
            />
          </div>
          <div className="field">
            <label>Email</label>
            <input
              type="email"
              placeholder="Email"
              value={this.state.email}
              onChange={(e) => this.setState({ email: e.target.value })}
            />
          </div>
          <button className="ui button blue">Edit</button>
        </form>
      </div>
    );
  }
}

export default EditContact;
