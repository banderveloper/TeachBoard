import React, {useState} from 'react';

const AuthPage: React.FC = () => {

    const [userName, setUserName] = useState('');
    const [password, setPassword] = useState('');

    return (
        <div>
            <input
                type="text"
                value={userName}
                onChange={e => setUserName(e.target.value)}
            />
            <input
                type="password"
                value={password}
                onChange={e => setPassword(e.target.value)}
            />
            <button type='submit'>Login</button>
        </div>
    );
};

export default AuthPage;