import { sleep, check } from 'k6';
import http from 'k6/http';
import { randomIntBetween } from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';

const pageSize = 100;
const baseUrl = 'http://localhost:632'




export const options = {
    stages: [
        { duration: '30s', target: 50 },
        { duration: '1m', target: 500 }, // simulate ramp-up of traffic from 1 to 100 users over 5 minutes.
        { duration: '1m', target: 0 }, // ramp-down to 0 users
        { duration: '5m', target: 50000 }, // ramp-down to 0 users
        { duration: '1m', target: 20 }, 
    ],
    thresholds: {
        http_req_failed: ['rate<0.01'], // http errors should be less than 1%
        http_req_duration: ['p(99)<300'], // 99% of requests must complete below 1.5s
    },
};

export function setup() {

}


export default function () {

    const query = `${baseUrl}/weatherforecast`;
    const res = http.get(query);

    sleep(1)
    check(res, { 'status was 200': (r) => r.status == 200 });
}