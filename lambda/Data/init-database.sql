CREATE TABLE IF NOT EXISTS events (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    date DATE NOT NULL,
    max_attendees INTEGER NOT NULL DEFAULT 0
);


CREATE TABLE IF NOT EXISTS participants (
    id SERIAL PRIMARY KEY,
    event_id INTEGER NOT NULL REFERENCES events(id) ON DELETE CASCADE,
    name VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL
);


CREATE INDEX IF NOT EXISTS idx_events_date ON events(date);
CREATE INDEX IF NOT EXISTS idx_participants_event_id ON participants(event_id);


CREATE UNIQUE INDEX IF NOT EXISTS idx_participants_event_email_unique ON participants(event_id, email);
