CREATE TABLE IF NOT EXISTS ratings
(
	id SERIAL PRIMARY KEY,
	movie_id INT NOT NULL,
	rating INT NOT NULL,
	FOREIGN KEY (movie_id) REFERENCES movies (id)
)
