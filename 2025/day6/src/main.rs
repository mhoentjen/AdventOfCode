use std::fs;
use std::str::{Chars, FromStr};

fn read_from_file_part_1(file_path: &str) -> Vec<Problem> {
    let contents = fs::read_to_string(file_path).expect("Should have been able to read the file");
    let mut operations: Vec<Operation> = vec![];
    let mut lines: Vec<Line_part1> = vec![];
    for source_line in contents.lines() {
        if source_line.contains('*') {
            operations = source_line
                .split_whitespace()
                .map(|x| x.parse().unwrap())
                .collect();
            break;
        }
        lines.push(Line_part1 {
            elements: source_line
                .split_whitespace()
                .into_iter()
                .map(|num| num.parse().unwrap())
                .collect(),
        });
    }

    let mut problems = vec![];
    for (index, operation) in operations.iter().enumerate() {
        problems.push(Problem {
            elements: lines
                .iter()
                .map(|line| line.elements[index].clone())
                .collect(),
            operation: operation.clone(),
        })
    }
    problems
}

fn read_from_file_part_2(file_path: &str) -> Vec<Problem> {
    let contents = fs::read_to_string(file_path).expect("Should have been able to read the file");
    let mut operations: Vec<Operation> = vec![];
    let mut lines: Vec<Line_part2> = vec![];
    let last_line = contents.lines().last().unwrap();
    let mut column_starts: Vec<u64> = Vec::new();
    for (index, char) in last_line.chars().enumerate() {
        if char == '*' || char == '+' {
            operations.push(operation_from_char(char));
            column_starts.push(index as u64);
        }
    }

    for source_line in contents.lines() {
        if source_line.contains('*') {
            break;
        }
        let mut new_line_elements: Vec<String> = Vec::new();

        for (index, column_start) in column_starts.iter().enumerate() {
            let column_end = if column_starts.len() > index + 1 {
                column_starts[index + 1] - 2
            } else {
                source_line.len() as u64
            };
            new_line_elements.push(
                source_line
                    .chars()
                    .skip(column_start.clone() as usize)
                    .take((column_end - column_start + 1) as usize)
                    .collect::<String>(),
            );
        }

        lines.push(Line_part2 {
            elements: new_line_elements,
        });
    }

    let mut problems = vec![];
    for (index, operation) in operations.iter().enumerate() {
        let field_width = lines[0].elements[index].chars().count();
        let mut numbers: Vec<u64> = Vec::new();
        for column in (0..field_width) {
            let mut number = 0;
            for line in &lines {
                let char = line.elements[index].chars().collect::<Vec<char>>()[column];
                match char {
                    ' ' => { continue },
                    '0'..='9' => { number = number * 10 + char.to_digit(10).unwrap() as u64 },
                    _ => {
                        panic!("Invalid character in number field")
                    }
                };
            }
            numbers.push(number);
        }
        
        problems.push(Problem {
            elements: numbers,
            operation: operation.clone(),
        })
    }
    problems
}

#[derive(Debug, Clone)]
struct Line_part1 {
    elements: Vec<u64>,
}

#[derive(Debug, Clone)]
struct Line_part2 {
    elements: Vec<String>,
}

fn operation_from_char(c: char) -> Operation {
    match c {
        '+' => Operation::Add,
        '*' => Operation::Multiply,
        _ => panic!("Invalid operation character"),
    }
}

impl FromStr for Operation {
    type Err = ();

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        match s.trim() {
            "+" => Ok(Operation::Add),
            "*" => Ok(Operation::Multiply),
            _ => Err(()),
        }
    }
}
#[derive(Debug, Clone)]
enum Operation {
    Add,
    Multiply,
}

#[derive(Debug)]
struct Problem {
    elements: Vec<u64>,
    operation: Operation,
}

struct Column {
    start: u64,
    end: u64,
}

fn main() {
    let file_path = "input.txt";
    let problems = read_from_file_part_2(file_path);
    let result = part1(&problems);
    println!("Part1 result: {:?}", result);
}

fn part1(problems: &Vec<Problem>) -> u64 {
    let mut sum = 0u64;
    for problem in problems {
        let result: u64 = match problem.operation {
            Operation::Add => problem.elements.iter().sum(),
            Operation::Multiply => problem.elements.iter().product(),
        };
        sum += result;
    }
    sum
}
