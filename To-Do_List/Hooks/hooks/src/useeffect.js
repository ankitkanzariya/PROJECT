import React, { useEffect, useState } from "react";

function Test() {
  const [content, setContent] = useState('posts');
  const [count, setCount] = useState(0);

  useEffect(() => {
    console.log('componentDidMount');
  }, []);

  useEffect(() => {
    console.log('componentDidUpdate');

    // Cleanup function
    return () => {
      console.log('unmount');
    };
  }, [content]);

  return (
    <div>
      <button onClick={() => setCount(count + 1)}>count {count}</button>

    <button onClick={() => setContent('Post')}> Post </button>
    <button onClick={() => setContent('User')}> User </button>
    <button onClick={() => setContent('Comment')}> Comment </button>
    

        <h1> {content} </h1>
    </div>
  ); 
}

export default Test;


// aa useeffect js che