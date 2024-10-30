import http from 'k6/http';
import { sleep } from 'k6';

export const options = {
    vus: 1,
    duration: '30s',
    cloud: {
        // Project: Default project
        projectID: 3721618,
        // Test runs with the same name groups test runs together.
        name: 'Test (30/10/2024-10:34:42)'
    }
};

export default function() {
    http.get('http://localhost:632/todos');
    sleep(1);
}