
async function refreshToken() {
    const refreshToken = getCookie('RefreshToken');
    console.log(`Current refresh token: ${refreshToken}`);
    const response = await fetch('https://localhost:5000/api/User/RefreshToken', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ refreshToken: refreshToken })
    });

    if (response.ok) {
        const data = await response.json();
        console.log('Refreshed token data:', data);
        setCookie('Token','savetoken');
        setCookie('RefreshToken', data.refreshToken);
    } else {
        console.log('Failed to refresh token');
    }
}

function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
}

//function setCookie(name, value, expiresIn) {
//    const date = new Date();
//    date.setTime(date.getTime() + (expiresIn * 1000));
//    document.cookie = `${name}=${value}; path=/; expires=${date.toUTCString()}`;
//    console.log(`Set cookie: ${name}=${value}; expires=${date.toUTCString()}`);
//}
function setCookie(name, value) {
    document.cookie = `${name}=${value}; path=/`;
    console.log(`Set cookie: ${name}=${value}`);
}


// Decode JWT token to check its expiration
function parseJwt(token) {
    try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));
        return JSON.parse(jsonPayload);
    } catch (e) {
        return null;
    }
}

async function ensureToken() {
    const token = getCookie('Token');
    if (token) {
        const tokenPayload = parseJwt(token);
        if (tokenPayload) {
            const now = Math.floor(Date.now() / 1000);
            const tokenExpiration = tokenPayload.exp;
            if (tokenExpiration - now < 60) { // refresh token if it will expire in the next 60 seconds
                await refreshToken();
            }
        } else {
            await refreshToken();
        }
    } else {
        await refreshToken();
    }
}

// Call ensureToken every 5 minutes to check and refresh token if necessary
setInterval(ensureToken, 120000); // 120000 ms = 2 minute

//FLUSHALL


