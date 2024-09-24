import React, { useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { addTask, removeTask, toggleTask } from './actions/todoActions';
import './App.css';

function App() {
  const [taskInput, setTaskInput] = useState('');
  const tasks = useSelector((state) => state.tasks);
  const dispatch = useDispatch();

  const handleAddTask = () => {
    if (taskInput.trim()) {
      dispatch(addTask(taskInput));
      setTaskInput('');
    }
  };

  return (
    <div className="container mt-5">
      <h1 className="text-center">Todo List</h1>
      <div className="input-group mb-3">
        <input
          type="text"
          className="form-control"
          value={taskInput}
          onChange={(e) => setTaskInput(e.target.value)}
          placeholder="Add a new task"
        />
        <div className="input-group-append">
          <button className="btn btn-primary" onClick={handleAddTask}>
            Add Task
          </button>
        </div>
      </div>
      <ul className="list-group">
        {tasks.map((task) => (
          <li key={task.id} className="list-group-item d-flex justify-content-between align-items-center">
            <span
              style={{ textDecoration: task.completed ? 'line-through' : 'none' }}
              onClick={() => dispatch(toggleTask(task.id))}
              className="flex-grow-1"
            >
              {task.text}
            </span>
            <button
              className="btn btn-danger btn-sm"
              onClick={() => dispatch(removeTask(task.id))}
            >
              Remove
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;
