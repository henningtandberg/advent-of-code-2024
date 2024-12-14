use std::fs;

fn main() {
    let contents = fs::read_to_string("src/puzzle.txt")
        .expect("Something went wrong reading the file");

    let map_tiles: Vec<Vec<MapTile>> = contents
        .lines()
        .enumerate()
        .map(|(i, line)| parse_map_tile(i as i32, line))
        .collect();

    let player_tile = map_tiles
        .iter()
        .flatten()
        .find(|tile| tile.tile_character == '^')
        .unwrap();

    let mut player = Player {
        current_coordinate: MapCoordinate {
            x: player_tile.coordinate.x,
            y: player_tile.coordinate.y,
        },
        current_direction: Direction::North,
        visited_coordinates: vec![ MapCoordinate {
            x: player_tile.coordinate.x,
            y: player_tile.coordinate.y,
        }],
    };

    //println!("Player: {:?}", player);
    while next_tile(&player, &map_tiles).is_some() {
        let next_tile = next_tile(&player, &map_tiles).unwrap();
        //println!("Next tile: {:?}", next_tile);

        match next_tile.tile_type {
            TileType::Open => {
                let next_coordinate = MapCoordinate {
                    x: next_tile.coordinate.x,
                    y: next_tile.coordinate.y,
                };
                player.current_coordinate.x = next_coordinate.x;
                player.current_coordinate.y = next_coordinate.y;
                let visited = player.visited_coordinates.iter().any(|coord| coord.x == next_coordinate.x && coord.y == next_coordinate.y);
                if !visited {
                    player.visited_coordinates.push(next_coordinate);
                }
            },
            TileType::Obstacle => {
                match player.current_direction {
                    Direction::North => {
                        let next_coordinate = MapCoordinate {
                            x: player.current_coordinate.x + 1,
                            y: player.current_coordinate.y,
                        };
                        player.current_coordinate.x = next_coordinate.x;
                        player.current_coordinate.y = next_coordinate.y;
                        let visited = player.visited_coordinates.iter().any(|coord| coord.x == next_coordinate.x && coord.y == next_coordinate.y);
                        if !visited {
                            player.visited_coordinates.push(next_coordinate);
                        }
                        player.current_direction = Direction::East;
                    },
                    Direction::East => {
                        let next_coordinate = MapCoordinate {
                            x: player.current_coordinate.x,
                            y: player.current_coordinate.y + 1,
                        };
                        player.current_coordinate.x = next_coordinate.x;
                        player.current_coordinate.y = next_coordinate.y;
                        let visited = player.visited_coordinates.iter().any(|coord| coord.x == next_coordinate.x && coord.y == next_coordinate.y);
                        if !visited {
                            player.visited_coordinates.push(next_coordinate);
                        }
                        player.current_direction = Direction::South;
                    },
                    Direction::South => {
                        let next_coordinate = MapCoordinate {
                            x: player.current_coordinate.x - 1,
                            y: player.current_coordinate.y,
                        };
                        player.current_coordinate.x = next_coordinate.x;
                        player.current_coordinate.y = next_coordinate.y;
                        let visited = player.visited_coordinates.iter().any(|coord| coord.x == next_coordinate.x && coord.y == next_coordinate.y);
                        if !visited {
                            player.visited_coordinates.push(next_coordinate);
                        }
                        player.current_direction = Direction::West;
                    },
                    Direction::West => {
                        let next_coordinate = MapCoordinate {
                            x: player.current_coordinate.x,
                            y: player.current_coordinate.y - 1,
                        };
                        player.current_coordinate.x = next_coordinate.x;
                        player.current_coordinate.y = next_coordinate.y;
                        let visited = player.visited_coordinates.iter().any(|coord| coord.x == next_coordinate.x && coord.y == next_coordinate.y);
                        if !visited {
                            player.visited_coordinates.push(next_coordinate);
                        }
                        player.current_direction = Direction::North;
                    },
                };
            },
        };
    }

    //for row in &map.map_tiles {
    //    for tile in row {
    //        let visited = player.visited_coordinates.iter().any(|coord| coord.x == tile.coordinate.x && coord.y == tile.coordinate.y);
    //        let character = if visited { 'X' } else { tile.tile_character };
    //        print!("{}", character);
    //    }
    //    println!();
    //}

    println!("Amount of visited coordinates: {:?}", player.visited_coordinates.len());
}

fn move_player (player: &mut Player, x: i32, y: i32, next_direction: Direction) {
    let next_coordinate = MapCoordinate { x, y };
    player.current_coordinate.x = next_coordinate.x;
    player.current_coordinate.y = next_coordinate.y;
    player.visited_coordinates.push(next_coordinate);
    player.current_direction = next_direction;
}

fn next_tile<'a>(player: &'a Player, map_tiles: &'a Vec<Vec<MapTile>>) -> Option<&'a MapTile> {
    let current_tile = map_tiles
        .get(player.current_coordinate.y as usize)
        .and_then(|row| row.get(player.current_coordinate.x as usize));

    match current_tile {
        Some(_tile) => {
            let next_coordinate = match player.current_direction {
                Direction::North => MapCoordinate {
                    x: player.current_coordinate.x,
                    y: player.current_coordinate.y - 1,
                },
                Direction::East => MapCoordinate {
                    x: player.current_coordinate.x + 1,
                    y: player.current_coordinate.y,
                },
                Direction::South => MapCoordinate {
                    x: player.current_coordinate.x,
                    y: player.current_coordinate.y + 1,
                },
                Direction::West => MapCoordinate {
                    x: player.current_coordinate.x - 1,
                    y: player.current_coordinate.y,
                },
            };

            let next_tile = map_tiles
                .get(next_coordinate.y as usize)
                .and_then(|row| row.get(next_coordinate.x as usize));

            match next_tile {
                Some(tile) => Some(tile),
                None => None,
            }
        },
        None => None,
    }
}

#[derive(Debug)]
struct MapCoordinate {
    x: i32,
    y: i32,
}

#[derive(Debug)]
struct MapTile {
    coordinate: MapCoordinate,
    tile_type: TileType,
    tile_character: char,
}

#[derive(Debug)]
enum TileType {
    Open,
    Obstacle,
}

#[derive(Debug)]
struct Player {
    current_coordinate: MapCoordinate,
    current_direction: Direction,
    visited_coordinates: Vec<MapCoordinate>,
}

#[derive(Debug)]
enum Direction {
    North,
    East,
    South,
    West,
}

fn parse_map_tile(row: i32, line: &str) -> Vec<MapTile> {
    line
        .chars()
        .enumerate()
        .map(|(col, character)| MapTile {
            coordinate: MapCoordinate {
                x: col as i32,
                y: row },
            tile_type: if character == '.' || character == '^' { TileType::Open } else { TileType::Obstacle },
            tile_character: character,
        }).collect()
}
